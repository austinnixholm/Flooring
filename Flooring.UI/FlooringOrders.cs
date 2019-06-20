using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.Models.Responses;
using Flooring.UI.IO;
using Flooring.UI.Workflows;

namespace Flooring.UI
{
    /// <summary>
    /// Represents the menu & workflow sequencing within the Flooring Manager UI
    /// </summary>
    public class FlooringOrders
    {
        private Manager _manager;
        private Output _output = new Output();
        private Input _input = new Input();

        public UIState State { get; set; } = UIState.Options;

        /// <summary>
        /// Starts the UI, and executes each loop based on the current UIState.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                switch (State)
                {
                    case UIState.Options:
                        //Instantiate a list of options from the Output class & print them
                        List<string> options = _output.DisplayOptions();
                        string input = _input.PromptUser("\n" + "Please enter an option: ");
                        //Prompt user to select an option from the list.
                        if (!int.TryParse(input, out int result))
                        {
                            _output.SendError("Please enter a valid option.");
                            continue;
                        }
                        //Check if the input is valid, depending on the size of the options list.
                        if (result < 1 || result > options.Count - 1)
                        {
                            _output.SendError($"Choice must be a value between 1 and {options.Count - 1}.");
                            continue;
                        }
                        //Convert the option # chosen to the corresponding UIState type.
                        State = (UIState) result;
                        break;
                    case UIState.DisplayOrders:
                        new DisplayOrderWorkflow().Execute(this, _output, _input);
                        break;
                    case UIState.AddOrder:
                        new AddOrderWorkflow().Execute(this, _output, _input);
                        break;
                    case UIState.EditOrder:
                        new EditOrderWorkflow().Execute(this, _output, _input);
                        break;
                    case UIState.RemoveOrder:
                        new RemoveOrderWorkflow().Execute(this, _output, _input);
                        break;
                    case UIState.Quit:
                        Console.Clear();
                        _output.SendTimedMessage("Thank you, goodbye.", ConsoleColor.Yellow, 1.25);
                        Environment.Exit(0);
                        break;
                }
            }
        }

    }

    public enum UIState { Options, DisplayOrders, AddOrder, EditOrder, RemoveOrder, Quit }
}
