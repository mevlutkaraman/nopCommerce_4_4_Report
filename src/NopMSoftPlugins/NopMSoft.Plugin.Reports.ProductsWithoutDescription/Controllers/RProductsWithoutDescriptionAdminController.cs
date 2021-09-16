using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Factories;
using NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Admin.Reports;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Controllers
{
    [Area(AreaNames.Admin)]
    [AuthorizeAdmin]
    [AutoValidateAntiforgeryToken]
    public class RProductsWithoutDescriptionAdminController : BasePluginController
    {
        #region Fields

         private readonly IReportModelFactory _reportModelFactory;
        private readonly IPermissionService _permissionService;

        #endregion

        #region Ctor

        public RProductsWithoutDescriptionAdminController(IReportModelFactory reportModelFactory,
                                                          IPermissionService permissionService)
        {
            _reportModelFactory = reportModelFactory;
            _permissionService = permissionService;
        }

        #endregion

        #region Methods

        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual async Task<IActionResult> List()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            //prepare model
            var model = await _reportModelFactory.PrepareReportSearchModelAsync(new ReportSearchModel());

            return View("~/Plugins/NopMSoft.Reports.ProductsWithoutDescription/Views/Admin/Reports/List.cshtml", model);
        }

        [HttpPost]
        public virtual async Task<IActionResult> ReportList(ReportSearchModel searchModel)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageSystemLog))
                return await AccessDeniedDataTablesJson();

            //prepare model
            var model = await _reportModelFactory.PrepareReportListModelAsync(searchModel);

            return Json(model);
        }


        [HttpPost, ActionName("ExportToExcel")]
        [FormValueRequired("exportexcel-all")]
        public virtual async Task<IActionResult> ExportExcelAll(ReportSearchModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            return View("");
        }

        [HttpPost]
        public virtual async Task<IActionResult> ExportExcelSelected(string selectedIds)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManageProducts))
                return AccessDeniedView();

            return View("");

        }

        #endregion
    }
}
