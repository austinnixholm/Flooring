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
    /// Represents the sequence & flow of user input when removing an order through the manager.
    /// </summary>
    public class RemoveOrderWorkflow : IWorkflow
    {
        /// <summary>
        /// Executes the sequence.
        /// </summary>
        /// <param name="ui">The Floor Manager UI instance</param>
        /// <param name="output">Output class instance</param>
        /// <param name="input">Input class instance</param>
        public void Execute(FlooringOrders ui, Output output, Input input)
        {
            string dateInput = string.Empty;
            int orderNumber = 0;
            Console.Clear();
            dateInput = input.PromptUser("Please enter the Date of the order (mm/dd/yyyy)");

            Manager manager = ManagerFactory.Create(dateInput);
            Response dateResponse = manager.ValidDate(manager.OrderRepository.OrderDate);
            //Check if the entered date is a valid entry, if not send an error
            if (dateResponse.ResponseType == ResponseType.Invalid)
            {
                output.SendError("Invalid date input.");
                return;
            }
            //Check if the entered date isn't later than today's date
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
                ui.State = UIState.Options;
                output.ShowFooter("Press any key to go back...");
                return;
            }
            //Display the orders to choose from to be deleted
            output.DisplayOrderData(manager.OrderRepository.Data, dateResponse.Date);

            //Prompt user for an order number
            string orderNumberInput = input.PromptUser("\n" + "Please enter the order number you'd like to remove.");
            //Check if string can be parsed as numeric value, and set the order number
            if (!int.TryParse(orderNumberInput, out orderNumber))
            {
                output.SendError("You can only enter numeric values for an order number.");
                return;
            }

            Order order = manager.OrderRepository.GetByValue(orderNumber);
            //Check if an order exists for that order number
            if (order == null)
            {
                output.SendError("Can't find order.");
                return;
            }

            //Attempt order removal & return a response
            Response removeResponse = manager.RemoveOrder(dateInput, orderNumber);
            ui.State = UIState.Options;
            //If the response failed, send the error & return to menu
            if (removeResponse.ResponseType.Equals(ResponseType.Fail) ||
                removeResponse.ResponseType.Equals(ResponseType.Invalid))
            {
                output.SendError(removeResponse.Message);
                return;
            }
            Console.Clear();
            //If success, a timed message will be sent & the user will automatically be sent to the menu
            output.SendTimedMessage("Order removed successfully!", ConsoleColor.Green, 1d);

        }
    }
}
