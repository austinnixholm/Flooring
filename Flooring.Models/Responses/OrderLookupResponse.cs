using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Responses
{
    /// <summary>
    /// Represents data & implementation of the response for looking up an order through logic.
    /// </summary>
    public class OrderLookupResponse : Response
    {
        public List<Order> Orders = new List<Order>();
    }
}
