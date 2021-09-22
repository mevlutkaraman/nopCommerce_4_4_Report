using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Domain.Catalog;
using Nop.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Framework.Extensions;
using Nop.Web.Framework.Models.Extensions;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Reports;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Factories
{
    /// <summary>
    /// Represents the report model factory
    /// </summary>
    public class ReportModelFactory : IReportModelFactory
    {
        #region Fields

        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly ICategoryService _categoryService;
        private readonly CatalogSettings _catalogSettings;
        private readonly ILocalizationService _localizationService;
        private readonly IPictureService _pictureService;
        private readonly IProductReportService _productReportService;
        private readonly IProductService _productService;

        #endregion

        #region Ctor

        public ReportModelFactory(IBaseAdminModelFactory baseAdminModelFactory, 
                                  ICategoryService categoryService,           
                                  CatalogSettings catalogSettings,
                                  ILocalizationService localizationService,
                                  IPictureService pictureService,
                                  IProductReportService productReportService,
                                  IProductService productService)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _categoryService= categoryService;
            _catalogSettings = catalogSettings;
            _localizationService = localizationService;
            _pictureService = pictureService;
            _productReportService = productReportService;
            _productService = productService;
        }

        #endregion

        #region Utilities



        #endregion


        #region Methods

        /// <summary>
        /// Prepare report search model
        /// </summary>
        /// <param name="searchModel">Report search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the report search model
        /// </returns>
        public async Task<ReportSearchModel> PrepareReportSearchModelAsync(ReportSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //prepare available categories
            await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

            //prepare available manufacturers
            await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

            //prepare available vendors
            await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

            //prepare available product types
            await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

            //prepare available warehouses
            await _baseAdminModelFactory.PrepareWarehousesAsync(searchModel.AvailableWarehouses);

            searchModel.HideStoresList = _catalogSettings.IgnoreStoreLimitations || searchModel.AvailableStores.SelectionIsNotPossible();
            searchModel.HideManufacturersList = searchModel.AvailableManufacturers.SelectionIsNotPossible();
            searchModel.HideVendorsList = searchModel.AvailableVendors.SelectionIsNotPossible();
            searchModel.HideWarehousesList = searchModel.AvailableWarehouses.SelectionIsNotPossible();
            searchModel.HideCategoriesList = searchModel.AvailableCategories.SelectionIsNotPossible();

            //prepare "published" filter (0 - all; 1 - published only; 2 - unpublished only)
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Admin.Catalog.Products.List.SearchPublished.All")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "1",
                Text = await _localizationService.GetResourceAsync("Admin.Catalog.Products.List.SearchPublished.PublishedOnly")
            });
            searchModel.AvailablePublishedOptions.Add(new SelectListItem
            {
                Value = "2",
                Text = await _localizationService.GetResourceAsync("Admin.Catalog.Products.List.SearchPublished.UnpublishedOnly")
            });

            //prepare description types

            searchModel.AvailableWithoutDescriptionTypes.Add(new SelectListItem
            {
                Value = "0",
                Text = await _localizationService.GetResourceAsync("Admin.Common.All")
            });

            //prepare available without description type
            var availableWithoutDescriptionTypeItems = await WithoutDescriptionType.ShortDescriptionOnly.ToSelectListAsync(false);
            foreach (var withoutDescriptionType in availableWithoutDescriptionTypeItems)
            {
                searchModel.AvailableWithoutDescriptionTypes.Add(withoutDescriptionType);
            }
            //prepare grid
            searchModel.SetGridPageSize();

            return searchModel;
        }

        /// <summary>
        /// Prepare paged report list model
        /// </summary>
        /// <param name="searchModel">Report search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the report list model
        /// </returns>
        public async Task<ReportListModel> PrepareReportListModelAsync(ReportSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));

            //get parameters to filter comments
            var published = searchModel.PublishedId == 0 ? null : (bool?)(searchModel.PublishedId == 1);

            var categoryIds = new List<int> { searchModel.CategoryId };
            if (searchModel.IncludeSubCategories && searchModel.CategoryId > 0)
            {
                var childCategoryIds = await _categoryService.GetChildCategoryIdsAsync(parentCategoryId: searchModel.CategoryId, showHidden: true);
                categoryIds.AddRange(childCategoryIds);
            }



            //get products
            var reportProducts = await _productReportService.GetReportProductsAsync(categoryIds: categoryIds,
                manufacturerId: searchModel.ManufacturerId,
                sku: searchModel.Sku,
                storeId: searchModel.StoreId,
                vendorId: searchModel.VendorId,
                warehouseId: searchModel.WarehouseId,
                productType: searchModel.ProductTypeId > 0 ? (ProductType?)searchModel.ProductTypeId : null,
                productName: searchModel.ProductName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                published: published,
                withoutDescriptionType: searchModel.WithoutDescriptionTypeId > 0 ? (WithoutDescriptionType?)searchModel.WithoutDescriptionTypeId : null);

            //prepare list model
            var model = await new ReportListModel().PrepareToGridAsync(searchModel, reportProducts, () =>
            {
                return reportProducts.SelectAwait(async reportProduct =>
                {
                    //fill in model values from the entity
                    var reportModel = new ReportModel 
                    {
                        Name=reportProduct.Name,
                        ProductId=reportProduct.Id,
                        Published=reportProduct.Published,
                        Sku=reportProduct.Sku,
                        ProductTypeId=reportProduct.ProductTypeId,
                    };

                    //fill in additional values (not existing in the entity)
                    var defaultProductPicture = (await _pictureService.GetPicturesByProductIdAsync(reportProduct.Id, 1)).FirstOrDefault();
                    
                    (reportModel.PictureThumbnailUrl, _) = await _pictureService.GetPictureUrlAsync(defaultProductPicture, 75);
                    reportModel.ProductTypeName = await _localizationService.GetLocalizedEnumAsync(reportProduct.ProductType);
                   
                    if (reportProduct.ProductType == ProductType.SimpleProduct && reportProduct.ManageInventoryMethod == ManageInventoryMethod.ManageStock)
                        reportModel.StockQuantityStr = (await _productService.GetTotalStockQuantityAsync(reportProduct)).ToString();

                    return reportModel;
                });
            });

            return model;
        }

        #endregion
    }
}
