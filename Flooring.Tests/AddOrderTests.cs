using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Models.Responses;
using NUnit.Framework;

namespace Flooring.Tests
{
    [TestFixture]
    public class AddOrderTests
    {
        //invalid tax test - cannot deliver to Alaska
        [TestCase("07062020", "Sample Customer", "Alaska", "Wood", 50.0, ResponseType.Fail)]
        //invalid product test - marble doesn't exist
        [TestCase("07062020", "Sample Customer", "Ohio", "Marble", 50.0, ResponseType.Fail)]
        //date must be greater than current
        [TestCase("07062017", "Sample Customer", "Ohio", "Wood", 50.0, ResponseType.Fail)]
        //invalid customer name, apostrophe
        [TestCase("07062020", "Bob's Shop", "Ohio", "Wood", 100.0, ResponseType.Fail)]
        //negative area decimal
        [TestCase("07062020", "Sample Customer", "Ohio", "Wood", -50.0, ResponseType.Fail)]
        //Area less than 100 sq feet
        [TestCase("07062020", "Sample Customer", "Ohio", "Wood", 50.0, ResponseType.Fail)]
        //Success - date 1
        [TestCase("07062020", "Sample Customer", "Ohio", "Wood", 100.0, ResponseType.Success)]
        //Success - date 2
        [TestCase("07/06/2020", "Sample Customer", "Ohio", "Wood", 150.0, ResponseType.Success)]
        public void TestAddOrder(string dateInput, string customerName, string state, string productType, decimal area,
            ResponseType expectedResult)
        {
            Manager manager = ManagerFactory.Create(dateInput);
            AddOrderResponse response = manager.AddOrder(customerName, state, productType, area);
            Assert.AreEqual(response.ResponseType, expectedResult);
        }
        

    }
}
