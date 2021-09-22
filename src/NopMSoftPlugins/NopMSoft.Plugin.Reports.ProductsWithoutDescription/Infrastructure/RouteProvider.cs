using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework;
using Nop.Web.Framework.Mvc.Routing;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Infrastructure
{
    /// <summary>
    /// Represents plugin route provider
    /// </summary>
    public class RouteProvider : IRouteProvider
    {
        /// <summary>
        /// Register routes
        /// </summary>
        /// <param name="endpointRouteBuilder">Route builder</param>
        public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
        {
            endpointRouteBuilder.MapControllerRoute(ProductsWithoutDescriptionReportDefults.ConfigurationRouteName,
             "Admin/Plugins/Reports/ProductsWithoutDescription/Configure",
                 new { controller = "RProductsWithoutDescriptionConfigure", action = "Configure", area = AreaNames.Admin });

            endpointRouteBuilder.MapControllerRoute(ProductsWithoutDescriptionReportDefults.ReportListRouteName,
              "Admin/Plugins/Reports/ProductsWithoutDescription/List",
                new { controller = "RProductsWithoutDescription", action = "List", area = AreaNames.Admin });
        }

        /// <summary>
        /// Gets a priority of route provider
        /// </summary>
        public int Priority => int.MaxValue;
    }
}
