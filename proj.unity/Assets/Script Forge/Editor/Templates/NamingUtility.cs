using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScriptForge.Utility
{
    /// <summary>
    /// A simple class used to make sure the name of objects is valid for compiled
    /// code.
    /// </summary>
    public static class NamingUtility
    {
        /// <summary>
        /// Removes all invalid characters from a string. Replaces any spaces with underscores. 
        /// </summary>
        /// <param name="name">The starting name that we are going to use.</param>
        /// <returns>The new variable safe name.</returns>
        public static string ToVariableName(string name)
        {
            // Capture our start
            char[] output = new char[name.Length];
            // Get the true length of our string. 
            int outputIndex = 0;
            // Loop over every letter and replace it if it's invalid
            for(int i = 0; i < name.Length; i++)
            {
                // Get our current letter
                char letter = name[i];
                // Check it's range
                if((letter >= 'a' && letter <= 'z') || (letter >= 'A' && letter <= 'Z'))
                {
                    // Set our value
                    output[outputIndex] = letter;
                    // Increase our index
                    outputIndex++;
                    // Move to the next element 
                    continue;
                }
                
                // Replace spaces with underscores
                if(letter == ' ')
                {
                    // Set our value
                    output[outputIndex] = '_';
                    // Increase our index
                    outputIndex++;
                    // Move to the next element 
                    continue;
                }
            }

            // return the result. 
            return new string(output, 0, outputIndex);
        }
    }
}
