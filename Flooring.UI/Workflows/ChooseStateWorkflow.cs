using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.BLL;
using Flooring.UI.IO;

namespace Flooring.UI.Workflows
{
    /// <summary>
    /// Represents the sequence & flow of user input when choosing a tax state from the repo.
    /// </summary>
    public class ChooseStateWorkflow
    {
        /// <summary>
        /// Prompts the user for option input, from a list of tax states pulled from the repository.
        /// </summary>
        /// <param name="output">The output instance</param>
        /// <param name="input">The input instance</param>
        /// <param name="manager">The manager from the BLL</param>
        /// <returns>A Tax state from the list chosen from validated input</returns>
        public string GetValue(Output output, Input input, Manager manager)
        {
            string state;
            //Display and create a list of available tax states to choose from
            List<string> states = output.DisplayAvailableTaxStates(manager.TaxRepository);

            state = input.PromptUser("Please choose a state from the list:");
            if (string.IsNullOrEmpty(state)) return state;
            //Check if the entry for the option is a number or not
            if (!int.TryParse(state, out int choice))
            {
                output.SendError("Please choose the number of the state you'd like to choose.");
                return string.Empty;
            }

            int maxChoice = states.Count;
            //Check if the entered choice is within the available tax state choices
            if (choice < 1 || choice > maxChoice)
            {
                output.SendError($"Invalid choice. Please choose between 1 and {maxChoice}.");
                return string.Empty;
            }

            //Select the state string form the generated list based on the entered choice
            state = states.ElementAt(choice - 1);
            return state;
        }

    }
}
