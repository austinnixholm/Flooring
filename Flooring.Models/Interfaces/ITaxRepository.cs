using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    /// <summary>
    /// Represents the implementation of properties and base methods for the TaxData type of a repository
    /// </summary>
    public interface ITaxRepository : IRepository<TaxData>
    {
        List<TaxData> Data { get; set; }

    }
}
