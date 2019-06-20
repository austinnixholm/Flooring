using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Data;
using Flooring.Models;
using Flooring.Models.Responses;
using Flooring.UI.IO;

namespace Flooring.UI.Workflows
{
    /// <summary>
    /// Represents the sequence & flow of user input when editing an order through the manager.
    /// </summary>
    public class EditOrderWorkflow : IWorkflow
    {
        /// <summary>
        /// Executes the sequence.
        /// </summary>
        /// <param name="ui">The Floor Manager UI instance</param>
        /// <param name="output">Output class instance</param>
        /// <param name="input">Input class instance</param>
        public void Execute(FlooringOrders ui, Output output, Input input)
        {
            string dateInput = string.Empty, customerName = string.Empty, state = string.Empty, productType = string.Empty;
            int orderNumber = 0;
            decimal area = 0;

            Console.Clear();
            Console.WriteLine("*** Edit Order **");

            //Prompt user to input a date
            dateInput = input.PromptUser("Please enter the Date of the order (mm/dd/yyyy)");

            Manager manager = ManagerFactory.Create(dateInput);
            Response dateResponse = manager.ValidDate(manager.OrderRepository.OrderDate);
            //Check if the entered date is a valid entry, if not send an error
            if (dateResponse.ResponseType == ResponseType.Invalid)
            {
                output.SendError("Invalid date input.");
                return;
            }
            //Check if the entered date is later than today's
            if (!(dateResponse.Date > DateTime.Now.Date))
            {
                output.SendError($"Date must be later than today's date: {DateTime.Today.Date:MM/dd/yyyy}");
                return;
            }

            Console.Clear();
            //Checks if the order repository for the inputted date contains any values
            if (manager.OrderRepository.Data.Count <= 0)
            {
                //If not, then the file should not exist.
                FileManager.DeleteFile(FileManager.OrdersPath, dateResponse.Date);
                output.SendTimedMessage("There are no orders to display.", ConsoleColor.Yellow, 1.5d);
                //Delete the file & return to main menu.
                ui.State = UIState.Options;
                output.ShowFooter("Press any key to go back...");
                return;
            }
            //Show the user the current order data
            output.DisplayOrderData(manager.OrderRepository.Data, dateResponse.Date);
            Order order = new Order();

            //Prompt the user for an order number until valid
            while (orderNumber == 0)
            {
                //Prompt user for an order number
                string orderNumberInput = input.PromptUser("Please enter the order number you'd like to edit.");
                //Check if string can be parsed as numeric value, and set the order number
                if (!int.TryParse(orderNumberInput, out orderNumber))
                {
                    output.SendError("You can only enter numeric values for an order number.");
                    continue;
                }
                order = manager.OrderRepository.GetByValue(orderNumber);
                //Check if an order exists for that order number
                if (order == null)
                {
                    output.SendError("No orders found.");
                    orderNumber = 0;
                }
            }
            //Prompt user to change the customer name
            while (string.IsNullOrEmpty(customerName))
            {
                customerName = input.PromptUser($"Edit current customer name? [{order.CustomerName}]");
                //If the user hits enter, the state returns to default
                if (string.IsNullOrEmpty(customerName)) break;
                //If the name is invalid, set the customer name to default and continue prompt
                if (!input.ValidName(customerName))
                {
                    output.SendError("Invalid customer name. Check special characters.");
                    customerName = string.Empty;
                }
            }

            //Prompt the user to edit the current state until some response is inputted
            while (string.IsNullOrEmpty(state))
            {
                Console.WriteLine($"Edit current state? [{order.TaxData.State}]" + "\n");
                //Compile list of states for user to choose from
                state = new ChooseStateWorkflow().GetValue(output, input, manager);
                //If the user hits enter, the state returns to default
                if (string.IsNullOrEmpty(state)) state = order.TaxData.State;
            }

            //Prompt the user to edit the current product type until a response
            while (string.IsNullOrEmpty(productType)) {
                Console.WriteLine($"Edit current product type? [{order.Product.ProductType}]" + "\n");
                //Compile list of product types for user to choose from
                productType = new ChooseProductWorkflow().GetProduct(output, input, manager);
                //If the user hits enter, the product type returns to default
                if (string.IsNullOrEmpty(productType)) productType = order.Product.ProductType;
            }

            Console.WriteLine($"Edit current area? [{order.Area}]");
            //Prompt the user to enter the area in sq ft until conditions are met
            while (area < 100)
            {
                string areaInput = input.PromptUser("Please enter the new area: ");
                //No input or hit enter - set area to original value
                if (string.IsNullOrEmpty(areaInput))
                {
                    area = order.Area;
                    break;
                }
                if (!decimal.TryParse(areaInput, out area))
                {
                    output.SendError("Could not parse input as decimal.");
                    continue;
                }
                if (area < 0)
                {
                    output.SendError("Area must be a positive decimal.");
                    continue;
                }
            }

            //Properly set any data that may be invalid
            string finalCustomerName = string.IsNullOrEmpty(customerName) ? order.CustomerName : customerName;
            string finalState = string.IsNullOrEmpty(state) ? order.TaxData.State : state;
            string finalProductType = string.IsNullOrEmpty(productType) ? order.Product.ProductType : productType;
            decimal finalArea = ((area != order.Area) && (area != 0 && area >= 100)) ? area : order.Area;

            //Attempt to edit the order & return a response
            EditOrderResponse response = manager.EditOrder(dateInput, order.OrderNumber, finalCustomerName, finalState,
                finalProductType, finalArea);
            ui.State = UIState.Options;
            //If the response fails, send the error & return to menu
            if (response.ResponseType == ResponseType.Fail || response.ResponseType == ResponseType.Invalid)
            {
                output.SendError(response.Message);
                return;
            }
            Console.Clear();
            //Display data, show footer & wait for input to return to menu
            output.DisplayOrderData(response.NewOrder, response.Date);
            output.SendTimedMessage("Order edited successfully!", ConsoleColor.Green, 1D);
            output.ShowFooter("Press any key to continue...");
        }
    }
}