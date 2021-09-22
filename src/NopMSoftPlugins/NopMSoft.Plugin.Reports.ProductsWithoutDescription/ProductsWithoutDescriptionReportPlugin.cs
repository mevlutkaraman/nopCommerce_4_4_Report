using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Menu;
using NopMSoft.Services.Reports;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription
{
    public class ProductsWithoutDescriptionReportPlugin : BasePlugin, IReportPlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;
        private readonly IUrlHelperFactory _urlHelperFactory;

        #endregion

        #region Ctor

        public ProductsWithoutDescriptionReportPlugin(IActionContextAccessor actionContextAccessor,
                                                      ILocalizationService localizationService,
                                                      ISettingService settingService,
                                                      IUrlHelperFactory urlHelperFactory)
        {
            _actionContextAccessor = actionContextAccessor;
            _localizationService = localizationService;
            _settingService = settingService;
            _urlHelperFactory = urlHelperFactory;
        }

        #endregion

        #region Methods

        public override string GetConfigurationPageUrl()
        {
            return _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext)
                                    .RouteUrl(ProductsWithoutDescriptionReportDefults.ConfigurationRouteName);
        }

        public async Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var nopMSoftNode = rootNode.ChildNodes.FirstOrDefault(node => node.SystemName == "NopMSoft");
            if(nopMSoftNode == null)
            {
                nopMSoftNode = new SiteMapNode
                {
                    Visible = true,
                    Title = await _localizationService.GetResourceAsync("Plugins.NopMSoft"),
                    IconClass = "fas fa-book",
                    SystemName = "NopMSoft"
                };
                rootNode.ChildNodes.Add(nopMSoftNode);
            }

            var reportsNode = nopMSoftNode.ChildNodes.FirstOrDefault(node => node.SystemName == "Reports");
            if(reportsNode== null)
            {
                reportsNode = new SiteMapNode
                {
                    Visible = true,
                    Title = await _localizationService.GetResourceAsync("Plugins.NopMSoft.Reports"),
                    IconClass = "fas fa-chart-line",
                    SystemName = "Reports"
                };
                nopMSoftNode.ChildNodes.Add(reportsNode);
            }

            var catalogNode = reportsNode.ChildNodes.FirstOrDefault(node => node.SystemName == "Catalog");
            if(catalogNode == null)
            {
                catalogNode = new SiteMapNode
                {
                    Visible = true,
                    Title = await _localizationService.GetResourceAsync("Plugins.NopMSoft.Reports.Catalog"),
                    IconClass = "fas fa-book",
                    SystemName = "Catalog"
                };
                reportsNode.ChildNodes.Add(catalogNode);
            }

            var reportNode = new SiteMapNode
            {
                Visible = true,
                Title = await _localizationService.GetResourceAsync("Plugins.NopMSoft.Reports.ProductsWithoutDescription"),
                IconClass = "far fa-dot-circle",
                SystemName = "ProductsWithoutDescriptionReport",
                RouteValues = new RouteValueDictionary { { "Area", "Admin" } },
                Url= _urlHelperFactory.GetUrlHelper(_actionContextAccessor.ActionContext).RouteUrl(ProductsWithoutDescriptionReportDefults.ReportListRouteName),
            };
            catalogNode.ChildNodes.Add(reportNode);
        }

        public override async Task InstallAsync()
        {
            //Locales
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.NopMSoft"] = "NopMSoft",
                ["Plugins.NopMSoft.Reports"] = "Reports",
                ["Plugins.NopMSoft.Reports.Catalog"] = "Catalog",
                ["Plugins.NopMSoft.Reports.ProductsWithoutDescription"] = "Products Without Description",
                ["Plugins.Reports.ProductsWithoutDescription.Fields.ScheduleTaskEnabled"] = "Schedule task enabled",
                ["Plugins.NopMSoft.Reports.ProductsWithoutDescription.List.SearchSku"] = "Sku",
                ["Plugins.NopMSoft.Reports.ProductsWithoutDescription.List.SearchWithoutDescriptionType"] = "Without description type",
                ["Plugins.NopMSoft.Reports.ProductsWithoutDescription.List.SearchWithoutDescriptionType.All"] = "All",
                ["Enums.NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services.WithoutDescriptionType.ShortDescriptionOnly"] = "Short description only",
                ["Enums.NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services.WithoutDescriptionType.FullDescriptionOnly"] = "Full description only",
            });

            await  base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<ProductsWithoutDescriptionReportSettings>();

            //locales
            await _localizationService.DeleteLocaleResourceAsync("Plugins.Reports.ProductsWithoutDescription.List.Sku");
            await _localizationService.DeleteLocaleResourceAsync("Plugins.NopMSoft.Reports.ProductsWithoutDescription");

            await base.UninstallAsync();
        }
        #endregion
    }
}
