using Microsoft.Extensions.DependencyInjection;
using Nop.Core.Configuration;
using Nop.Core.Infrastructure;
using Nop.Core.Infrastructure.DependencyManagement;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Factories;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Infrastructure
{
    /// <summary>
    /// Represents a plugin dependency registrar
    /// </summary>
    public class DependencyRegistrar : IDependencyRegistrar
    {

        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="typeFinder">Type finder</param>
        /// <param name="appSettings">App settings</param>
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppSettings appSettings)
        {
            //Services
            services.AddScoped<IProductReportService, ProductReportService>();
            services.AddScoped<IExportManager, ExportManager>();

            //Factories
            services.AddScoped<IReportModelFactory, ReportModelFactory>();
        }

        /// <summary>
        /// Order of this dependency registrar implementation
        /// </summary>
        public int Order => int.MaxValue;
    }
}
