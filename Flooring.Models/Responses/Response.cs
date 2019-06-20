using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Responses
{
    /// <summary>
    /// Represents base properties of a response modified through the BLL
    /// </summary>
    public class Response
    {
        public ResponseType ResponseType { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
    }

    public enum ResponseType { Success, Fail, Invalid }
}
