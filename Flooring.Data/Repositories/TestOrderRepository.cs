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
    /// Represents a test order repository to be used within Unit tests, on the production config
    /// </summary>
    public class TestOrderRepository : IOrderRespository
    {
        public List<Order> Data { get; set; } = new List<Order>();
        public string OrderDate { get; set; }

        public TestOrderRepository(string orderDate)
        {
            OrderDate = orderDate.Replace("/", "");
            GenerateTestOrder();
        }

        /// <summary>
        /// Creates sample data for the repository
        /// </summary>
        private void GenerateTestOrder()
        {
            TaxData tax = new TaxData();
            tax.State = "Ohio";
            tax.StateAbbreviation = "OH";
            tax.TaxRate = 6.25M;
            
            Product product = new Product();
            product.ProductType = "Carpet";
            product.CostPerSquareFoot = 2.25M;
            product.LaborCostPerSquareFoot = 2.10M;

            Order order = Order.CreateOrder("Test Customer", 200M, tax, product, Data);
            Data.Add(order);
        }

        public List<Order> GetAll()
        {
            return Data;
        }

        public List<Order> GetAllByValue(object value)
        {
            return !(value is string) ? null : Data.Where(o => o.CustomerName.ToLower().Equals(((string)value).ToLower())).ToList();
        }

        public Order GetByValue(object value)
        {
            return !(value is int) ? null : Data.FirstOrDefault(o => o.OrderNumber.Equals((int)value));
        }

        public void Add(Order obj)
        {
            if (Data.Any(o => o.OrderNumber == obj.OrderNumber)) return;
            Data.Add(obj);
        }

        public void Update(Order obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].OrderNumber.Equals(obj.OrderNumber))
                    Data[i] = obj;
        }

        public void Delete(object value)
        {
            if (!(value is string)) return;
            Data.Remove(Data.FirstOrDefault(o => o.OrderNumber.Equals((int)value)));
        }

        public void Save()
        {
            //test repos shouldn't save
        }
    }
}
