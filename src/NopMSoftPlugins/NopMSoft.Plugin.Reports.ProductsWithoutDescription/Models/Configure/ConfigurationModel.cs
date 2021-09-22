using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Models.Configure
{

    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Reports.ProductsWithoutDescription.Fields.ScheduleTaskEnabled")]
        public bool ScheduleTaskEnabled { get; set; }
        public bool ScheduleTaskEnabled_OverrideForStore { get; set; }
    }
}
