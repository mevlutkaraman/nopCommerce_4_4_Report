using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Configure;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Controllers
{
    [AuthorizeAdmin]
    [Area(AreaNames.Admin)]
    [AutoValidateAntiforgeryToken]
    public class RProductsWithoutDescriptionConfigureController : BasePluginController
    {
        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public RProductsWithoutDescriptionConfigureController(ILocalizationService localizationService,
                                                         INotificationService notificationService,
                                                         IPermissionService permissionService,
                                                         ISettingService settingService,
                                                         IStoreContext storeContext)
        {
            _localizationService = localizationService;
            _notificationService = notificationService;
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods 

        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var productsWithoutDescriptionReportSettings = await _settingService.LoadSettingAsync<ProductsWithoutDescriptionReportSettings>(storeScope);

            var model = new ConfigurationModel
            {
                Enabled = productsWithoutDescriptionReportSettings.Enabled,
            };

            if (storeScope > 0)
            {
                model.Enabled_OverrideForStore = await _settingService.SettingExistsAsync(productsWithoutDescriptionReportSettings, x => x.Enabled, storeScope);
            }

            return View("~/Plugins/NopMSoft.Reports.ProductsWithoutDescription/Views/Configure/Configure.cshtml", model);
        }

        [HttpPost]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var productsWithoutDescriptionReportSettings = await _settingService.LoadSettingAsync<ProductsWithoutDescriptionReportSettings>(storeScope);

            //save settings
            productsWithoutDescriptionReportSettings.Enabled = model.Enabled;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */

            await _settingService.SaveSettingOverridablePerStoreAsync(productsWithoutDescriptionReportSettings, x => x.Enabled, model.Enabled_OverrideForStore, storeScope, false);
            

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }

        #endregion
    }
}
