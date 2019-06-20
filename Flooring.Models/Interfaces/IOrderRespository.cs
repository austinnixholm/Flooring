using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    /// <summary>
    /// Implements base methods & Properties of an Order Repository into a single interface.
    /// Usage: production/file repositories
    /// </summary>
    public interface IOrderRespository : IRepository<Order>, IOrderProperties
    {
    }
}
