using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Admin.Reports
{
    public record ReportSearchModel : BaseSearchModel
    {
        #region Ctor

        public ReportSearchModel()
        {

        }

        #endregion

        #region Properties  

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductName")]
        public string ProductName { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchCategory")]
        public int CategoryId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchIncludeSubCategories")]
        public bool IncludeSubCategories { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchManufacturer")]
        public int ManufacturerId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchStore")]
        public int StoreId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchVendor")]
        public int VendorId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchWarehouse")]
        public int WarehouseId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchProductType")]
        public int ProductTypeId { get; set; }

        [NopResourceDisplayName("Admin.Catalog.Products.List.SearchPublished")]
        public int PublishedId { get; set; }

        [NopResourceDisplayName("Plugins.Reports.ProductsWithoutDescription.List.Sku")]
        public string Sku { get; set; }


        public IList<SelectListItem> AvailableCategories { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailableManufacturers { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailableStores { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailableWarehouses { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailableVendors { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailableProductTypes { get; set; } = new List<SelectListItem>();

        public IList<SelectListItem> AvailablePublishedOptions { get; set; } = new List<SelectListItem>();


        public bool HideStoresList { get; set; }

        public bool HideManufacturersList { get; set; }

        public bool HideVendorsList { get; set; }

        public bool HideWarehousesList { get; set; }

        #endregion
    }
}
