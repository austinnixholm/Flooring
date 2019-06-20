using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models.Interfaces
{
    /// <summary>
    /// Base implementation of a repository class & basic CRUD
    /// </summary>
    /// <typeparam name="T">The object type for the repository</typeparam>
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();
        List<T> GetAllByValue(object value);
        T GetByValue(object value);
        void Add(T obj);
        void Update(T obj);
        void Delete(object value);
        void Save();
    }
}
