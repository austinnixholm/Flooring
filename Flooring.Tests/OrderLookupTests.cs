using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Data;
using Flooring.Data.Repositories;
using Flooring.Models.Responses;
using NUnit.Framework;

namespace Flooring.Tests
{
    [TestFixture]
    public class OrderLookupTests
    {

        [TestCase("06012013", ResponseType.Success)] //valid date format
        [TestCase("06/01/2013", ResponseType.Success)] //valid date format 2
        [TestCase("06.01.2013", ResponseType.Invalid)] //invalid date format
        public void LookupOrderTest(string date, ResponseType expected)
        {
            OrderLookupResponse response = new OrderLookupResponse();
            Manager manager = new Manager(new TestOrderRepository(date));
            response = manager.LookupOrders();

            Assert.AreEqual(expected, response.ResponseType);
        }

    }
}
