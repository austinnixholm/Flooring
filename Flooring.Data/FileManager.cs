using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flooring.Data
{
    /// <summary>
    /// A class that handles miscellaneous file operations that may be accessed statically.
    /// </summary>
    public static class FileManager
    {
        public static readonly string OrderFileHeader = "[OrderNumber, CustomerName, State, TaxRate, ProductType, Area, CostPerSquareFoot, LaborCostPerSquareFoot, MaterialCost, LaborCost, Tax, Total]";
        public static readonly string OrdersPath = @"C:\Users\pelic\source\repos\Flooring\Orders\Orders_";
        public static readonly string Separator = "~";

        /// <summary>
        /// Deletes a file specified by the date, within a path.
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="date">The date on the file</param>
        public static void DeleteFile(string path, DateTime date)
        {
            string filePath = GetFilePath(path, date);
            if (!File.Exists(filePath)) return;

            File.Delete(filePath);
        }

        /// <summary>
        /// Constructs a file path from a date object
        /// </summary>
        /// <param name="filePath">The file path</param>
        /// <param name="date">The date on file</param>
        /// <returns>The resulting file path</returns>
        public static string GetFilePath(string filePath, DateTime date)
        {
            return filePath + date.ToString("MMddyyyy") + ".txt";
        }

    }
}
