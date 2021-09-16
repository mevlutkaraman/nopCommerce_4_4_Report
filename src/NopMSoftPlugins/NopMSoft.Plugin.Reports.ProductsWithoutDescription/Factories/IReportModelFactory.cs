using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Admin.Reports;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Factories
{
    /// <summary>
    /// Represents the report model factory
    /// </summary>
    public interface IReportModelFactory
    {
        /// <summary>
        /// Prepare report search model
        /// </summary>
        /// <param name="searchModel">Report search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the report search model
        /// </returns>
        Task<ReportSearchModel> PrepareReportSearchModelAsync(ReportSearchModel searchModel);

        /// <summary>
        /// Prepare paged report list model
        /// </summary>
        /// <param name="searchModel">Report search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the report list model
        /// </returns>
        Task<ReportListModel> PrepareReportListModelAsync(ReportSearchModel searchModel);
    }
}
