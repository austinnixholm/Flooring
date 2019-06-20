using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Models.Responses;
using Flooring.UI.IO;

namespace Flooring.UI.Workflows
{
    /// <summary>
    /// Represents the sequence & flow of user input when looking up an order through the manager.
    /// </summary>
    public class DisplayOrderWorkflow : IWorkflow
    {
        /// <summary>
        /// Executes the sequence.
        /// </summary>
        /// <param name="ui">The Floor Manager UI instance</param>
        /// <param name="output">Output class instance</param>
        /// <param name="input">Input class instance</param>
        public void Execute(FlooringOrders ui, Output output, Input input)
        {
            Console.Clear();
            Console.WriteLine("*** Display Orders ***");
            string userInput = input.PromptUser("Please enter the Date of the order (mm/dd/yyyy)");

            //Create repository manager to open order files from this date
            Manager manager = ManagerFactory.Create(userInput);
            OrderLookupResponse response = manager.LookupOrders();

            if (response == null)
            {
                //In this case, the date was invalid
                output.SendError("Invalid response.");
                return;
            }
            //Check the response for errors
            if (response.ResponseType == ResponseType.Fail || response.ResponseType == ResponseType.Invalid)
            {
                //If the order lookup fails, send the response message & return to menu
                output.SendError(response.Message);
                ui.State = UIState.Options;
                return;
            }

            //Display orders as long as the repository is not empty
            if (response.Orders.Count > 0)
                output.DisplayOrderData(response.Orders, response.Date);
            else
                output.SendTimedMessage("\n" + $"No orders found on the date: {response.Date:MM/dd/yyyy}", ConsoleColor.Yellow, 0.75);
            
            //Show footer & return to menu
            output.ShowFooter("Press any key to go back...");
            ui.State = UIState.Options;
        }

    }
}
