using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Responses
{
    /// <summary>
    /// Represents data & implementation of the response for editing an order through logic.
    /// </summary>
    public class EditOrderResponse : Response
    {
        public Order NewOrder { get; set; }
    }
}
