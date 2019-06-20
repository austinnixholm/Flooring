using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Data;
using Flooring.Models;
using Flooring.Models.Interfaces;
using Flooring.Models.Responses;

namespace Flooring.UI.IO
{
    /// <summary>
    /// Handles relevant Console Output methods & displaying data
    /// </summary>
    public class Output
    {
        /// <summary>
        /// Sends a timed message in error format.
        /// </summary>
        /// <param name="errorMessage">The message</param>
        public void SendError(string errorMessage)
        {
            SendTimedMessage($"Error: {errorMessage}", ConsoleColor.Red, 1D);
        }

        /// <summary>
        /// Displays a numbered list of options in a list.
        /// </summary>
        /// <returns>A list of options</returns>
        public List<string> DisplayOptions()
        {
            List<string> options = new List<string>(){"Display Data", "Add an Order", "Edit an Order", "Remove an Order", "Quit"};

            Console.Clear();
            
            for (int i = 0; i < options.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {options[i]}");
            }

            return options;
        }

        /// <summary>
        /// Method that displays a footer message for a workflow & waits for a key to be inputted before returning
        /// </summary>
        /// <param name="message">The footer message</param>
        public void ShowFooter(string message)
        {
            Console.WriteLine("\n" + message);
            Console.ReadKey();
        }

        /// <summary>
        /// Queries a list of unique product types from a Product repository & returns a list
        /// </summary>
        /// <param name="repository">The Product repo</param>
        /// <returns>A list of unique product types</returns>
        public List<string> DisplayAvailableProductTypes(IProductRepository repository)
        {
            List<string> types = (from p in repository.Data
                where !string.IsNullOrEmpty(p.ProductType)
                select p.ProductType).Distinct().ToList();
            int count = 1;
            foreach (string s in types)
            {
                Console.WriteLine("\t" + $"{count++}. {s}");
            }

            return types;
        }

        /// <summary>
        /// Queries a list of TaxD states from a TaxData repository & returns a list
        /// </summary>
        /// <param name="repository">The TaxData repo</param>
        /// <returns>A list of unique tax states to choose from</returns>
        public List<string> DisplayAvailableTaxStates(ITaxRepository repository)
        {
            List<string> states = (from t in repository.Data
                where !string.IsNullOrEmpty(t.State)
                select t.State).Distinct().ToList();
            int count = 1;
            foreach (string s in states)
            {
                Console.WriteLine("\t" + $"{count++}. {s}");
            }

            return states;
        }

        /// <summary>
        /// Displays Order data in a consistent format.
        /// </summary>
        /// <param name="order">The order instance</param>
        /// <param name="date">The date of the order</param>
        public void DisplayOrderData(Order order, DateTime date)
        {
            //Console.Clear();
            Console.WriteLine($"ORDER #{order.OrderNumber} | DATE: {date:MM/dd/yyyy}");
            Console.WriteLine("\t" + $"{order.CustomerName}");
            Console.WriteLine("\t" + $"{order.TaxData.State}");
            Console.WriteLine("\t" + $"Product: {order.Product.ProductType}");
            Console.WriteLine("\t" + $"Area: {order.Area}");
            Console.WriteLine("\t" + $"Materials: {order.MaterialCost:c}");
            Console.WriteLine("\t" + $"Labor: {order.LaborCost:c}");
            Console.WriteLine("\t" + $"Tax: {order.Tax:c}");
            Console.WriteLine("\t" + $"Total: {order.Total:c}");
            Console.WriteLine();
        }

        /// <summary>
        /// Displays a list of Order data in a consistent format
        /// </summary>
        /// <param name="orders">The list of Orders</param>
        /// <param name="date">The date of this order repo</param>
        public void DisplayOrderData(List<Order> orders, DateTime date)
        {
            Console.Clear();
            foreach (Order order in orders)
            {
                DisplayOrderData(order, date);
            }
        }

        /// <summary>
        /// Sends a message, and tells the thread to sleep an amount of seconds.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="color">The color to change the message to</param>
        /// <param name="timer">A decimal of seconds for the message</param>
        public void SendTimedMessage(string message, ConsoleColor color, double timer)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep((int) (timer * 1000));
        }

    }
}
