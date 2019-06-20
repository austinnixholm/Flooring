using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.UI.IO
{
    /// <summary>
    /// Handles relevant console input functions from the user.
    /// </summary>
    public class Input
    {
        public readonly List<char> AllowedChars = new List<char>() {',', '.', ' '};

        /// <summary>
        /// Print a message and return a read line from the console.
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <returns>User inputted string</returns>
        public string PromptUser(string message)
        {
            Console.WriteLine(message);
            return Console.ReadLine();
        }

        /// <summary>
        /// Checks a string if it's valid to parse as a customer name
        /// </summary>
        /// <param name="input">The string input</param>
        /// <returns>False if non-valid special characters</returns>
        public bool ValidName(string input)
        {
            char[] chars = input.ToCharArray();

            for (int i = 0; i < chars.Length; i++)
            {
                if (char.IsDigit(chars[i]) || char.IsLetter(chars[i])) continue;

                if (!AllowedChars.Contains(chars[i]))
                    return false;
            }

            return true;
        }

    }
}
