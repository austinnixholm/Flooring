using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data.Repositories
{
    /// <summary>
    /// Represents a test product repository to be used for Unit testing
    /// </summary>
    public class TestProductRepository : IProductRepository
    {
        public List<Product> Data { get; set; }

        public TestProductRepository()
        {
            Data = new List<Product>()
            {
                new Product()
                {
                    ProductType = "Wood",
                    CostPerSquareFoot = 5.15M,
                    LaborCostPerSquareFoot = 4.75M
                }
            };
        }

        public List<Product> GetAll()
        {
            return Data;
        }

        public List<Product> GetAllByValue(object value)
        {
            return !(value is string) ? null : Data.Where(p => p.ProductType.ToLower() == ((string)value).ToLower()).ToList();
        }

        public Product GetByValue(object value)
        {
            return !(value is string) ? null : Data.FirstOrDefault(p => p.ProductType.ToLower() == ((string)value).ToLower());
        }

        public void Add(Product obj)
        {
            if (Data.Any(p => p.ProductType.ToLower() == obj.ProductType.ToLower())) return;
            Data.Add(obj);
        }

        public void Update(Product obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].ProductType.ToLower().Equals(obj.ProductType.ToLower()))
                    Data[i] = obj;
        }

        public void Delete(object value)
        {
            if (!(value is string)) return;
            Data.Remove(Data.FirstOrDefault(p => p.ProductType.ToLower() == ((string)value).ToLower()));
        }

        public void Save()
        {
            //Test repos shouldn't save
            throw new NotImplementedException();
        }
    }
}
