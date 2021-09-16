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
    public class ReportModelFactory : IReportModelFactory
    {
        public Task<ReportListModel> PrepareReportListModelAsync(ReportSearchModel searchModel)
        {
            throw new NotImplementedException();
        }

        public Task<ReportSearchModel> PrepareReportSearchModelAsync(ReportSearchModel searchModel)
        {
            throw new NotImplementedException();
        }
    }
}
