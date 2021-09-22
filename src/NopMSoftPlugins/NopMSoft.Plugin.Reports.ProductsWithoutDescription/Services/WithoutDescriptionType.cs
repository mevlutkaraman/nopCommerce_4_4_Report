using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopMSoft.Plugin.Reports.ProductsWithoutDescription.Services
{
    /// <summary>
    /// Represents a without description type
    /// </summary>
    public enum WithoutDescriptionType
    {
        /// <summary>
        /// Short description
        /// </summary>
        ShortDescriptionOnly = 5,

        /// <summary>
        /// Full description
        /// </summary>
        FullDescriptionOnly = 10,
    }
}
