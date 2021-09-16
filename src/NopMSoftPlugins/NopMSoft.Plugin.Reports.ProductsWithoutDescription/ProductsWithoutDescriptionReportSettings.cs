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
        ///Gets or sets report enabled value. 
        /// </summary>
        public bool Enabled { get; set; }
    }
}
