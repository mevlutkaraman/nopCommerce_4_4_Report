using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription
{
    public class ProductsWithoutDescriptionReportDefults
    {

        /// <summary>
        /// Gets the plugin system name
        /// </summary>
        public static string SystemName => "Reports.ProductsWithoutDescription";

        /// <summary>
        /// Gets the user agent used to request third-party services
        /// </summary>
        public static string UserAgent => $"nopCommerce-{NopVersion.CURRENT_VERSION}";

        /// <summary>
        /// Gets the configuration route name
        /// </summary>
        public static string ConfigurationRouteName => "Plugin.Reports.ProductsWithoutDescriptionReport.Configure";

        /// <summary>
        /// Gets the report list route name
        /// </summary>
        public static string ReportListRouteName => "Plugin.Reports.ProductsWithoutDescriptionReport.List";
    }
}
