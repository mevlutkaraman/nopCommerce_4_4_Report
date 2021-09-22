using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services
{
    /// <summary>
    /// Export manager interface
    /// </summary>
    public interface IExportManager
    {
        /// <summary>
        /// Export report products to XLSX
        /// </summary>
        /// <param name="products">Products</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<byte[]> ExportReportProductsToXlsxAsync(IEnumerable<Product> products);
    }
}
