using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services
{
    /// <summary>
    /// Product report service interface
    /// </summary>
    public interface IProductReportService
    {

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
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" products
        /// false - load only "Unpublished" products
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the products
        /// </returns>
        Task<IPagedList<Product>> GetWithoutDescriptionReportAsync(int pageIndex = 0,int pageSize = int.MaxValue,
                                                                           IList<int> categoryIds = null,
                                                                           int manufacturerId = 0,
                                                                           string sku= "",int storeId = 0,
                                                                           int vendorId = 0, int warehouseId = 0, 
                                                                           ProductType? productType = null, string productName = "",
                                                                           bool? overridePublished = null);
    }
}
