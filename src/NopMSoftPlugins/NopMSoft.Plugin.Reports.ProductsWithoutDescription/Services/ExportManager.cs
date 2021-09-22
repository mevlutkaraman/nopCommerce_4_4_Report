using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Services.Catalog;
using Nop.Services.ExportImport.Help;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services
{
    /// <summary>
    /// Expoer manager
    /// </summary>
    public class ExportManager : IExportManager
    {
        #region Fields

        private readonly CatalogSettings _catalogSettings;
        private readonly ICategoryService _categoryService;
        private readonly IManufacturerService _manufacturerService;

        #endregion

        #region Ctor

        public ExportManager(CatalogSettings catalogSettings,
                             ICategoryService categoryService,
                             IManufacturerService manufacturerService)
        {
            _catalogSettings = catalogSettings;
            _categoryService = categoryService;
            _manufacturerService = manufacturerService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Returns the list of categories for a product separated by a ";"
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of categories
        /// </returns>
        protected virtual async Task<object> GetCategoriesAsync(Product product)
        {
            string categoryNames = null;
            foreach (var pc in await _categoryService.GetProductCategoriesByProductIdAsync(product.Id, true))
            {
                var category = await _categoryService.GetCategoryByIdAsync(pc.CategoryId);
                categoryNames += _catalogSettings.ExportImportProductCategoryBreadcrumb
                    ? await _categoryService.GetFormattedBreadCrumbAsync(category)
                    : category.Name;

                categoryNames += ";";
            }

            return categoryNames;
        }

        /// <summary>
        /// Returns the list of manufacturer for a product separated by a ";"
        /// </summary>
        /// <param name="product">Product</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of manufacturer
        /// </returns>
        protected virtual async Task<object> GetManufacturersAsync(Product product)
        {
            string manufacturerNames = null;
            foreach (var pm in await _manufacturerService.GetProductManufacturersByProductIdAsync(product.Id, true))
            {
                var manufacturer = await _manufacturerService.GetManufacturerByIdAsync(pm.ManufacturerId);
                manufacturerNames += manufacturer.Name;

                manufacturerNames += ";";
            }

            return manufacturerNames;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Export report products to XLSX
        /// </summary>
        /// <param name="products">Products</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public async Task<byte[]> ExportReportProductsToXlsxAsync(IEnumerable<Product> products)
        {
            var properties = new[] 
            {
                new PropertyByName<Product>("ProductId", p => p.Id),
                new PropertyByName<Product>("Name", p => p.Name),
                new PropertyByName<Product>("SKU", p => p.Sku),
                new PropertyByName<Product>("ManufacturerPartNumber", p => p.ManufacturerPartNumber),
                new PropertyByName<Product>("Gtin", p => p.Gtin),
                new PropertyByName<Product>("Categories", GetCategoriesAsync),
                new PropertyByName<Product>("Manufacturers", GetManufacturersAsync),
                new PropertyByName<Product>("ShortDescription", p => string.IsNullOrEmpty(p.ShortDescription)),
                new PropertyByName<Product>("FullDescription", p => string.IsNullOrEmpty(p.FullDescription)),
            };

            var productList = products.ToList();

            return await new PropertyManager<Product>(properties, _catalogSettings).ExportToXlsxAsync(productList);
        }

        #endregion
    }
}
