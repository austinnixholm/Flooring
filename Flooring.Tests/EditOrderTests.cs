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
    public class EditOrderTests
    {
        //invalid order number 5 doesn't exist
        [TestCase("07022020", 5, "New Name", "Ohio", "Wood", 100.0, ResponseType.Fail)]
        //invalid name
        [TestCase("07022020", 1, "Special Name!", "Ohio", "Wood", 100.0, ResponseType.Fail)]
        //invalid tax info
        [TestCase("07022020", 1, "New Name", "Alaska", "Wood", 100.0, ResponseType.Fail)]
        //invalid product info "Marble"
        [TestCase("07022020", 1, "New Name", "Ohio", "Marble", 100.0, ResponseType.Fail)]
        //negative area
        [TestCase("07022020", 1, "New Name", "Ohio", "Wood", -100.0, ResponseType.Fail)]
        //invalid date input
        [TestCase("07-02-2020", 1, "New Name", "Ohio", "Wood", 100.0, ResponseType.Invalid)]
        //new area less than 100 sq feet
        [TestCase("07022020", 1, "New Name", "Ohio", "Wood", 75.0, ResponseType.Fail)]
        //successful
        [TestCase("07022020", 1, "New Name", "Ohio", "Wood", 100.0, ResponseType.Success)]
        public void TestEditOrder(string inputtedDate, int orderNumber, string newCustomerName, string newState,
            string newProductType, decimal newArea, ResponseType expectedResult)
        {
            Manager manager = ManagerFactory.Create(inputtedDate);
            EditOrderResponse response = manager.EditOrder(inputtedDate, orderNumber, newCustomerName, newState,
                newProductType, newArea);
            Assert.AreEqual(expectedResult, response.ResponseType);
        }

    }
}
