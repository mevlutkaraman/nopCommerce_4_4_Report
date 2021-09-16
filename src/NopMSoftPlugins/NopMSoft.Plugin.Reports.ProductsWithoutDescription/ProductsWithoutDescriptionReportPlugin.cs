using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
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
        }

        public override async Task InstallAsync()
        {
            //Locales
            await _localizationService.AddLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Reports.ProductsWithoutDescription.List.Sku"] = "Sku",
                ["Plugins.NopMSoft"] = "NopMSoft",
                ["Plugins.NopMSoft.Reports"] = "Reports",
                ["Plugins.NopMSoft.Reports.Catalog"] = "Catalog",
            });

            await  base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<ProductsWithoutDescriptionReportSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Reports.ProductsWithoutDescription.List.Sku");

            await base.UninstallAsync();
        }
        #endregion
    }
}
