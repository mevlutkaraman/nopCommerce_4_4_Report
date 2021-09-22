using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Factories;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Reports;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class RProductsWithoutDescriptionController : BasePluginController
    {
        #region Fields

         private readonly ICategoryService _categoryService;
         private readonly IExportManager _exportManager;
         private readonly INotificationService _notificationService;
         private readonly IReportModelFactory _reportModelFactory;
         private readonly IPermissionService _permissionService;
         private readonly IProductReportService _productReportService;

        #endregion

        #region Ctor

        public RProductsWithoutDescriptionController(ICategoryService categoryService,
                                                     IExportManager exportManager,   
                                                     INotificationService notificationService,
                                                     IReportModelFactory reportModelFactory,
                                                     IPermissionService permissionService,
                                                     IProductReportService productReportService)
        {
            _categoryService = categoryService;
            _exportManager = exportManager;
            _notificationService = notificationService;
            _reportModelFactory = reportModelFactory;
            _permissionService = permissionService;
            _productReportService = productReportService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareReportSearchModelAsync(new ReportSearchModel());

            return View("~/Plugins/NopMSoft.Reports.ProductsWithoutDescription/Views/Reports/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ReportList(ReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareReportListModelAsync(searchModel);

            return Json(model);
        }


        [HttpPost, ActionName("ExportToExcel")]
        [FormValueRequired("exportexcel-all")]
        public virtual async Task<IActionResult> ExportExcelAll(ReportSearchModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //get parameters to filter comments
            var published = model.PublishedId == 0 ? null : (bool?)(model.PublishedId == 1);

            var categoryIds = new List<int> { model.CategoryId };
            if (model.IncludeSubCategories && model.CategoryId > 0)
            {
                var childCategoryIds = await _categoryService.GetChildCategoryIdsAsync(parentCategoryId: model.CategoryId, showHidden: true);
                categoryIds.AddRange(childCategoryIds);
            }

            //get products
            var reportProducts = await _productReportService.GetReportProductsAsync(categoryIds: categoryIds,
                manufacturerId: model.ManufacturerId,
                sku: model.Sku,
                storeId: model.StoreId,
                vendorId: model.VendorId,
                warehouseId: model.WarehouseId,
                productType: model.ProductTypeId > 0 ? (ProductType?)model.ProductTypeId : null,
                productName: model.ProductName,
                pageIndex: model.Page - 1, pageSize: model.PageSize,
                published: published,
                withoutDescriptionType: model.WithoutDescriptionTypeId > 0 ? (WithoutDescriptionType?)model.WithoutDescriptionTypeId : null);

            try
            {
                var bytes = await _exportManager.ExportReportProductsToXlsxAsync(reportProducts);
                return File(bytes, MimeTypes.TextXlsx, "WitoutDescriptionProductReport.xlsx");
            }
            catch (Exception exc)
            {
                await _notificationService.ErrorNotificationAsync(exc);
                return RedirectToRoute(ProductsWithoutDescriptionReportDefults.ReportListRouteName);
            }
        }

        #endregion
    }
}
