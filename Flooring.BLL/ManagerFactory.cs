using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Data;
using Flooring.Data.Repositories;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.BLL
{
    public static class ManagerFactory
    {
        /// <summary>
        /// Creates a manager instance depending on the current app configuration.
        /// </summary>
        /// <param name="date">The date passed for the order repository.</param>
        /// <returns>A new Manager instance</returns>
        public static Manager Create(string date)
        {
            string mode = ConfigurationManager.AppSettings["Mode"];

            switch (mode)
            {
                case "File":
                    return new Manager(new OrderRepository(date));
                case "Prod":
                    Manager manager = new Manager(new TestOrderRepository(date));
                    manager.TaxRepository = new TestTaxRepository();
                    manager.ProductRepository = new TestProductRepository();
                    return manager;
                default:
                    throw new Exception("Mode value in app config is not valid");
            }
        }

    }
}
