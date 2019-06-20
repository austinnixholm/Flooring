using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Models
{
    /// <summary>
    /// Represents an active Order for the flooring manager.
    /// </summary>
    public class Order
    {
        public int OrderNumber { get; set; }
        public string CustomerName { get; set; }
        public TaxData TaxData { get; set; }
        public Product Product { get; set; }
        public decimal Area { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }

        /// <summary>
        /// Generates an Order with a pre-defined orderNumber
        /// </summary>
        /// <param name="name">The customer name</param>
        /// <param name="area">The area in sq ft.</param>
        /// <param name="tax">The tax data of this order</param>
        /// <param name="product">The product data of this order</param>
        /// <param name="orderNumber">The order #</param>
        /// <returns>A new Order object instantiated with parameter values</returns>
        public static Order CreateOrder(string name, decimal area, TaxData tax, Product product, int orderNumber)
        {
            Order order = new Order();
            order.OrderNumber = orderNumber;
            order.CustomerName = name;
            order.Area = area;
            order.TaxData = tax;
            order.Product = product;
            order.MaterialCost = area * product.CostPerSquareFoot;
            order.LaborCost = area * product.LaborCostPerSquareFoot;
            order.Tax = ((order.MaterialCost + order.LaborCost) * (order.TaxData.TaxRate / 100));
            order.Total = (order.MaterialCost + order.LaborCost + order.Tax);

            return order;
        }

        /// <summary>
        /// Generates an order with a new order number, based on the passed in List of orders
        /// </summary>
        /// <param name="name">The customer name</param>
        /// <param name="area">The area in sq ft.</param>
        /// <param name="tax">The tax data for the order</param>
        /// <param name="product">The product data for the order</param>
        /// <param name="orders">The list of existing orders in the repo</param>
        /// <returns>A new Order object</returns>
        public static Order CreateOrder(string name, decimal area, TaxData tax, Product product, List<Order> orders)
        {
            Order order = new Order();
            order.OrderNumber = GetNextOrderNumber(orders);
            order.CustomerName = name;
            order.Area = area;
            order.TaxData = tax;
            order.Product = product;
            order.MaterialCost = area * product.CostPerSquareFoot;
            order.LaborCost = area * product.LaborCostPerSquareFoot;
            order.Tax = ((order.MaterialCost + order.LaborCost) * (order.TaxData.TaxRate / 100));
            order.Total = (order.MaterialCost + order.LaborCost + order.Tax);

            return order;
        }

        /// <summary>
        /// Determines the next order number for a newly generated order should it be added to a list.
        /// </summary>
        /// <param name="data">The List of orders in the repo</param>
        /// <returns>A generated order number</returns>
        private static int GetNextOrderNumber(List<Order> data)
        {
            List<int> orderNumbers = (from o in data
                                      where o.OrderNumber != -1
                                      select o.OrderNumber).ToList();
            return orderNumbers.Count == 0 ? 1 : orderNumbers.Max() + 1;
        }

    }
}
