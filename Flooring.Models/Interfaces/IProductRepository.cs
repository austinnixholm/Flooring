using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    /// <summary>
    /// Implements methods for an IRepository of type Product, and contains the List property
    /// </summary>
    public interface IProductRepository : IRepository<Product>
    {
        List<Product> Data { get; set; }
    }
}
