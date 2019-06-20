using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Eventing.Reader;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Data;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.BLL
{
    public class Manager
    {
        public ITaxRepository TaxRepository = new TaxRepository();
        public IProductRepository ProductRepository = new ProductRepository();
        public IOrderRespository OrderRepository;

        public Manager(IOrderRespository repo)
        {
            OrderRepository = repo;
        }

        /// <summary>
        /// Attempts to remove an order from the current IOrderRepository data
        /// </summary>
        /// <param name="inputtedDate">The date to parse & search for</param>
        /// <param name="orderNumber">The specific order number to delete within the file</param>
        /// <returns>A validated response</returns>
        public Response RemoveOrder(string inputtedDate, int orderNumber)
        {
            Response response = new Response();

            ParseDate(response, inputtedDate);
            if (response.ResponseType.Equals(ResponseType.Invalid)) return response;

            Order order = OrderRepository.GetByValue(orderNumber);
            if (order == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Could not find order.";
                return response;
            }
            OrderRepository.Delete(orderNumber);
            response.ResponseType = ResponseType.Success;
            return response;
        }

        /// <summary>
        /// Attempts to add an order to the current IOrderRepository data
        /// </summary>
        /// <param name="customerName">The customer name</param>
        /// <param name="state">The state</param>
        /// <param name="productType">The product type</param>
        /// <param name="area">The sq ft area</param>
        /// <returns>A validated response</returns>
        public AddOrderResponse AddOrder(string customerName, string state, string productType, decimal area)
        {
            AddOrderResponse response = new AddOrderResponse();
            DateTime date = ParseDate(response, OrderRepository.OrderDate);

            //If the date couldn't be parsed...
            if (response.ResponseType.Equals(ResponseType.Invalid)) return response;

            TaxData tax = TaxRepository.GetByValue(state);
            Product product = ProductRepository.GetByValue(productType);
            
            if (tax == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = $"We can't sell to the state: {state}";
            } else if (product == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = $"Product doesn't exist: {productType}";
            } else if (!(date > DateTime.Now.Date))
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = $"Date must be later than today's date: {DateTime.Today.Date:MM/dd/yyyy}";
            }  else if (!ValidNameInput(customerName))
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Invalid customer name input.";
            } else if (area < 0)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Area value must be a positive decimal.";
            } else if (area < 100)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Area value must be 100 or higher.";
            }

            if (response.ResponseType != ResponseType.Fail && response.ResponseType != ResponseType.Invalid)
            {
                Order order = Order.CreateOrder(customerName, area, tax, product, OrderRepository.Data);
                OrderRepository.Add(order);

                response.Order = order;
                response.Date = date;
                response.ResponseType = ResponseType.Success;
            }
            else
            {
                if (OrderRepository.Data.Count <= 0)
                    FileManager.DeleteFile(FileManager.OrdersPath, date);
            }
            return response;
        }

        /// <summary>
        /// Attempts to edit an order within the current IOrderRepository data
        /// </summary>
        /// <param name="inputtedDate"></param>
        /// <param name="orderNumber"></param>
        /// <param name="newCustomerName"></param>
        /// <param name="newState"></param>
        /// <param name="newProductType"></param>
        /// <param name="newArea"></param>
        /// <returns></returns>
        public EditOrderResponse EditOrder(string inputtedDate, int orderNumber, string newCustomerName, string newState,
            string newProductType, decimal newArea)
        {
            EditOrderResponse response = new EditOrderResponse();
            DateTime date = ParseDate(response, inputtedDate);
            if (response.ResponseType.Equals(ResponseType.Invalid)) return response;

            Order order = OrderRepository.GetByValue(orderNumber);
            if (order == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Couldn't find order with that number.";
                return response;
            }
            if (!ValidNameInput(newCustomerName))
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Invalid customer name. Check special characters.";
                return response;
            }
            TaxData tax = TaxRepository.GetByValue(newState);
            Product product = ProductRepository.GetByValue(newProductType);
            if (tax == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Invalid state.";
                return response;
            }
            if (product == null)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Invalid product type.";
                return response;
            }

            if (newArea < 0)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Area must be a positive decimal.";
                return response;
            }

            if (newArea < 100)
            {
                response.ResponseType = ResponseType.Fail;
                response.Message = "Area must be at least 100 sq ft.";
                return response;
            }

            Order updated = Order.CreateOrder(newCustomerName, newArea, tax, product, orderNumber);
            OrderRepository.Update(updated);

            response.NewOrder = updated;
            response.Date = date;
            response.ResponseType = ResponseType.Success;
            return response;
        }

        /// <summary>
        /// Attempts to lookup orders from the current IOrderRepository file.
        /// </summary>
        /// <returns>A validated response</returns>
        public OrderLookupResponse LookupOrders()
        {
            OrderLookupResponse response = new OrderLookupResponse();
            DateTime date = ParseDate(response, OrderRepository.OrderDate);
            if (response.ResponseType.Equals(ResponseType.Invalid)) return response;

            response.Date = date;
            response.Orders = OrderRepository.Data;
            response.ResponseType = ResponseType.Success;
            return response;
        }

        /// <summary>
        /// Parses a string date and takes in a response to validate.
        /// </summary>
        /// <param name="response">The response</param>
        /// <param name="inputtedDate">The string of the date</param>
        /// <returns>Parsed DateTime object</returns>
        private DateTime ParseDate(Response response, string inputtedDate)
        {
            DateTime date = new DateTime();
            try
            {
                date = DateTime.ParseExact(inputtedDate, "MMddyyyy", CultureInfo.InvariantCulture);
                response.Date = date;
                if (!(date > DateTime.Now.Date))
                {
                    FileManager.DeleteFile(FileManager.OrdersPath, date);
                }
            }
            catch (Exception e)
            {
                response.ResponseType = ResponseType.Invalid;
                response.Message = "Please enter a valid Date.";
            }

            return date;
        }

        /// <summary>
        /// Checks if a string is parseable as a date
        /// </summary>
        /// <param name="date">The string to parse</param>
        /// <returns>A validated response</returns>
        public Response ValidDate(string date)
        {
            Response response = new Response();
            ParseDate(response, date);

            return response;
        }

        /// <summary>
        /// Checks a string for special characters.
        /// </summary>
        /// <param name="name">The name to check</param>
        /// <returns>True if the string contains no invalid special chars</returns>
        private bool ValidNameInput(string name)
        {
            char[] allowedSpecialChars = {',', '.', ' '};
            char[] chars = name.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsDigit(chars[i]) || char.IsLetter(chars[i])) continue;

                if (!allowedSpecialChars.Contains(chars[i]))
                    return false;
            }

            return true;
        }
    }
}
