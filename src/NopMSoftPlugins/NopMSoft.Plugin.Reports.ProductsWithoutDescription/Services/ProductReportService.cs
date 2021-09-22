using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Services.Stores;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services
{
    /// <summary>
    /// Product report service
    /// </summary>
    public class ProductReportService : IProductReportService
    {
        #region Fields

        private readonly IRepository<ProductCategory> _productCategoryRepository;
        private readonly IRepository<ProductManufacturer> _productManufacturerRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductWarehouseInventory> _productWarehouseInventoryRepository;
        private readonly IStoreMappingService _storeMappingService;

        #endregion

        #region Ctor

        public ProductReportService(IRepository<ProductCategory> productCategoryRepository,   
                                    IRepository<ProductManufacturer> productManufacturerRepository,
                                    IRepository<Product> productRepository,
                                    IRepository<ProductWarehouseInventory> productWarehouseInventoryRepository,
                                    IStoreMappingService storeMappingService)
        {
            _productCategoryRepository = productCategoryRepository;
            _productManufacturerRepository = productManufacturerRepository;
            _productRepository = productRepository;
            _productWarehouseInventoryRepository= productWarehouseInventoryRepository;
            _storeMappingService = storeMappingService;
        }

        #endregion

        /// <summary>
        /// Product without description report
        /// </summary>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="categoryIds">Category identiferies</param>
        /// <param name="manufacturerId">Manufacturer identifiers</param>
        /// <param name="sku">A value indicating whether to search by a specified product SKU</param>
        /// <param name="storeId">Store identifiers</param>
        /// <param name="vendorId">Vendor identifiers</param>
        /// <param name="warehouseId">Warehouse identifier</param>
        /// <param name="productType">Product type; 0 to load all records</param>
        /// <param name="productName">Product name</param>
        /// <param name="published">
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" products
        /// false - load only "Unpublished" products
        /// </param>
        ///<param name="withoutDescriptionType">Filter description type</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        public async Task<IPagedList<Product>> GetReportProductsAsync(int pageIndex = 0, int pageSize = int.MaxValue, 
                                                                          IList<int> categoryIds = null, int manufacturerId = 0, string sku = "", 
                                                                          int storeId = 0, int vendorId = 0, int warehouseId = 0, 
                                                                          ProductType? productType = null, string productName = "", 
                                                                          bool? published = null,WithoutDescriptionType? withoutDescriptionType = null)
        {

            //some databases don't support int.MaxValue
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            var query = _productRepository.Table;

            query = query.Where(p => !p.Deleted);

            //category
            if (categoryIds is not null)
            {
                if (categoryIds.Contains(0))
                    categoryIds.Remove(0);

                if(categoryIds.Any())
                {
                    query = from product in query
                            join productCategory in _productCategoryRepository.Table on product.Id equals productCategory.ProductId
                            where categoryIds.Contains(productCategory.CategoryId)
                            select product;
                }
            }

            //manufactrer
            if (manufacturerId > 0)
            {
                query = from product in query
                        join productManufacturer in _productManufacturerRepository.Table on product.Id equals productManufacturer.ProductId
                        where productManufacturer.ManufacturerId == manufacturerId
                        select product;
            }

            //Sku
            if (!string.IsNullOrEmpty(sku))
                query = query.Where(p => p.Sku == sku);

            //apply store mapping constraints
            query = await _storeMappingService.ApplyStoreMapping(query, storeId);

            //vendor
            if (vendorId > 0)
              query= query.Where(p=>p.VendorId == vendorId);

            //warehouse
            if (warehouseId > 0)
            {
                query = query.Where(p => !p.UseMultipleWarehouses ? p.WarehouseId == warehouseId :
                                 _productWarehouseInventoryRepository.Table.Any(pwi => pwi.Id == warehouseId && pwi.ProductId == p.Id));
            }

            //product type
            if (productType.HasValue)
                query = query.Where(p => p.ProductTypeId == (int)productType);

            //product name
            if (!string.IsNullOrEmpty(productName))
                query = query.Where(p => p.Name.Contains(productName));

            //published
            if (published.HasValue)
                query = query.Where(p => p.Published == published.Value);

            if (!withoutDescriptionType.HasValue)
                query = query.Where(p => string.IsNullOrEmpty(p.FullDescription) || string.IsNullOrEmpty(p.ShortDescription));
            else
            {
                if (withoutDescriptionType == WithoutDescriptionType.ShortDescriptionOnly)
                    query = query.Where(p => string.IsNullOrEmpty(p.ShortDescription));
                else
                    query = query.Where(p => string.IsNullOrEmpty(p.FullDescription));
            }

            query = query.OrderByDescending(p => p.DisplayOrder);

            return await query.ToPagedListAsync(pageIndex, pageSize);
        }
    }
}
