using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models
{
    /// <summary>
    /// Represents properties necessary for TaxData of an order
    /// </summary>
    public class TaxData
    {
        public string StateAbbreviation { get; set; }
        public string State { get; set; }
        public decimal TaxRate { get; set; }
    }
}
