using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    /// <summary>
    /// Implements properties specific to an IRepository<Order>
    /// </summary>
    public interface IOrderProperties
    {
        List<Order> Data { get; set; }
        string OrderDate { get; set; }
    }
}
