using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Models;
using Flooring.Models.Responses;
using Flooring.UI.IO;

namespace Flooring.UI.Workflows
{
    /// <summary>
    /// Represents the sequence & flow of user input when adding an order through the manager.
    /// </summary>
    public class AddOrderWorkflow : IWorkflow
    {
        /// <summary>
        /// Executes the sequence.
        /// </summary>
        /// <param name="ui">The Floor Manager UI instance</param>
        /// <param name="output">Output class instance</param>
        /// <param name="input">Input class instance</param>
        public void Execute(FlooringOrders ui, Output output, Input input)
        {
            string date = string.Empty, customerName = string.Empty, state = string.Empty, productType = string.Empty;
            decimal area = 0;
            Console.Clear();
            Console.WriteLine("*** Add an Order ***");

            date = input.PromptUser("Enter a date for your order (mm/dd/yyyy):");
            if (string.IsNullOrEmpty(date)) return;

            //Create repository manager to open order files from this date
            Manager manager = ManagerFactory.Create(date);
            Response dateResponse = manager.ValidDate(manager.OrderRepository.OrderDate);

            //Check if the entered date is a valid entry, if not send an error
            if (dateResponse.ResponseType == ResponseType.Invalid)
            {
                output.SendError("Invalid date input.");
                return;
            }
            //Check if the date is later than today's
            if (!(dateResponse.Date > DateTime.Now.Date))
            {
                output.SendError($"Date must be later than today's date: {DateTime.Today.Date:MM/dd/yyyy}");
                return;
            }
            //Query customer's name
            while (string.IsNullOrEmpty(customerName))
            {
                customerName = input.PromptUser("Please enter a name for your order:");
                if (string.IsNullOrEmpty(customerName)) continue;
                if (!input.ValidName(customerName))
                {
                    output.SendError("Invalid customer name. Check special characters.");
                    customerName = string.Empty;
                }
            }

            //Prompt the user for input from list of states, repeat until valid input
            while (string.IsNullOrEmpty(state))
            {
                state = new ChooseStateWorkflow().GetValue(output, input, manager);
            }
            //Prompt the user for input from list of product types, repeat until valid input
            while (string.IsNullOrEmpty(productType))
            {
                productType = new ChooseProductWorkflow().GetProduct(output, input, manager);
            }

            //Prompt the user for input of the area of the order in sq ft., repeat until valid input
            while (area < 100)
            {
                string areaInput = input.PromptUser("Enter the area dimensions for this product:");
                if (string.IsNullOrEmpty(areaInput)) continue;
                if (!decimal.TryParse(areaInput, out area))
                {
                    output.SendError("Could not parse input as decimal.");
                    continue;
                }

                if (area < 0)
                {
                    output.SendError("Area must be a positive decimal.");
                    continue;
                } if (area < 100)
                {
                    output.SendError("Area must have a sq ft of over 100.");
                    continue;
                }
            }
            Console.Clear();
            string confirm = string.Empty;
            //Prompt the user to confirm their order until input is valid.
            while (string.IsNullOrEmpty(confirm))
            {
                confirm = input.PromptUser("Are you sure you want to add this order? (y/n)");
                if (!string.IsNullOrEmpty(confirm) && (!confirm.ToLower().Equals("y") && !confirm.ToLower().Equals("n")))
                {
                    confirm = string.Empty;
                }
            }
            //If the user declines, return to the main menu
            if (confirm.ToLower().Equals("n"))
            {
                output.SendTimedMessage("Returning to options.", ConsoleColor.Yellow, 1.0d);
                ui.State = UIState.Options;
                return;
            }
            //Once all values are properly inputted, attempt to add the order & get a response
            AddOrderResponse response = manager.AddOrder(customerName, state, productType, area);
            ui.State = UIState.Options;
            if (response.ResponseType.Equals(ResponseType.Fail) || response.ResponseType.Equals(ResponseType.Invalid))
            {
                output.SendError(response.Message);
                return;
            }
            Console.Clear();
            //Display data & return to main menu
            output.DisplayOrderData(response.Order, response.Date);
            output.SendTimedMessage("Order added successfully!", ConsoleColor.Green, 1D);
            output.ShowFooter("Press any key to continue...");

        }

    }
}
