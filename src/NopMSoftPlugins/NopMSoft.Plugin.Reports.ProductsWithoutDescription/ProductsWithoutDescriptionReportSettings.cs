using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription
{
    /// <summary>
    /// Represents settings of product without description plugin
    /// </summary>
    public class ProductsWithoutDescriptionReportSettings : ISettings
    {
        /// <summary>
        ///Gets or sets report schedule task enabled value. 
        /// </summary>
        public bool ScheduleTaskEnabled { get; set; }

    }

}
