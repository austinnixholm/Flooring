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
    /// Represents the sequence & flow of user input when adding an choosing a product type from the repo.
    /// </summary>
    public class ChooseProductWorkflow
    {
        /// <summary>
        /// Prompts the user for option input, from a list of product types pulled from the repository.
        /// </summary>
        /// <param name="output">The output instance</param>
        /// <param name="input">The input instance</param>
        /// <param name="manager">The manager from the BLL</param>
        /// <returns>A product from the list chosen from validated input</returns>
        public string GetProduct(Output output, Input input, Manager manager)
        {
            string productType;
            //Display and create a list of available product types to choose from
            List<string> productTypes = output.DisplayAvailableProductTypes(manager.ProductRepository);
            productType = input.PromptUser("Please choose a product type from the list: ");
            if (string.IsNullOrEmpty(productType)) return productType;

            //Check if the entry for the option is a number or not
            if (!int.TryParse(productType, out int choice))
            {
                output.SendError("Please choose the number of the product type you'd like to choose.");
                return string.Empty;
            }

            int maxChoice = productTypes.Count;

            //Check if the entered choice is within the available product type choices
            if (choice < 1 || choice > maxChoice)
            {
                output.SendError($"Invalid choice. Please choose between 1 and {maxChoice}.");
                return string.Empty;
            }

            //Select the product type string form the generated list based on the entered choice
            productType = productTypes.ElementAt(choice - 1);
            return productType;
        }

    }
}
