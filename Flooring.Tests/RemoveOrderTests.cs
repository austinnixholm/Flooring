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
    public class RemoveOrderTests
    {
        //Success case
        [TestCase("07022020", 1, ResponseType.Success)]
        //Invalid order #
        [TestCase("07022020", 5, ResponseType.Fail)]
        //Invalid date entry
        [TestCase("07-02-2020", 1, ResponseType.Invalid)]
        public void TestRemoveOrder(string inputtedDate, int customerNumber, ResponseType expectedResult)
        {
            Manager manager = ManagerFactory.Create(inputtedDate);
            Response response = manager.RemoveOrder(inputtedDate, customerNumber);

            Assert.AreEqual(expectedResult, response.ResponseType);
        }

    }
}
