using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data
{
    public class TaxRepository : ITaxRepository
    {
        public readonly string FileHeader = "[StateAbbreviation,State,TaxRate]";
        public readonly string FilePath = @"C:\Users\pelic\source\repos\Flooring\Taxes.txt";
        public readonly string Separator = FileManager.Separator;

        public List<TaxData> Data { get; set; }

        public TaxRepository()
        {
            Data = GetAll();
        }

        /// <summary>
        /// Maps each line of the file within the file path to a TaxData object & compiles a list.
        /// </summary>
        /// <returns>List of products from the file</returns>
        public List<TaxData> GetAll()
        {
            List<TaxData> data = new List<TaxData>();
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.ToLower().Equals(FileHeader.ToLower())) continue;
                    TaxData tax = MapToData(line);
                    if (tax != null) data.Add(tax);
                }
            }

            return data;
        }

        /// <summary>
        /// Queries a list of TaxData objects by state.
        /// </summary>
        /// <param name="value">The state</param>
        /// <returns>The queried list</returns>
        public List<TaxData> GetAllByValue(object value)
        {
            return !(value is string) ? null : Data.Where(t => t.State.ToLower() == ((string)value).ToLower()).ToList();
        }

        /// <summary>
        /// Retrieves an individual TaxData object from the data by state.
        /// </summary>
        /// <param name="value">The state</param>
        /// <returns>The retrieved data</returns>
        public TaxData GetByValue(object value)
        {
            return !(value is string) ? null : Data.FirstOrDefault(t => t.State.ToLower() == ((string)value).ToLower());
        }

        /// <summary>
        /// Adds a TaxData object to the data list
        /// </summary>
        /// <param name="obj">The TaxData to add</param>
        public void Add(TaxData obj)
        {
            if (Data.Any(t => t.StateAbbreviation.ToLower() == obj.State.ToLower())) return;
            Data.Add(obj);
        }

        /// <summary>
        /// Deletes a value by state from the current repository.
        /// </summary>
        /// <param name="value">The state</param>
        public void Delete(object value)
        {
            if (!(value is string)) return;
            Data.Remove(Data.FirstOrDefault(t => t.State.ToLower() == ((string)value).ToLower()));
        }

        /// <summary>
        /// Splits a string into individual values and parses them into a TaxData object.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <returns>A TaxData object based on the values</returns>
        private TaxData MapToData(string s)
        {
            string[] split = s.Split(Convert.ToChar(Separator));
            if (split.Length != 3) return null;

            TaxData data = new TaxData();
            data.StateAbbreviation = split[0];
            data.State = split[1];
            data.TaxRate = decimal.Parse(split[2]);

            return data;
        }

        /// <summary>
        /// Converts a TaxData object into a parseable string to be saved to data.
        /// </summary>
        /// <param name="data">The TaxData object to convert</param>
        /// <returns>A parseable TaxData string</returns>
        private string MapToRow(TaxData data)
        {
            string row = string.Empty;
            row += data.StateAbbreviation + Separator;
            row += data.State + Separator;
            row += data.TaxRate;

            return row;
        }

        /// <summary>
        /// Saves the Data list to file
        /// </summary>
        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(FileHeader);
                foreach (TaxData t in Data)
                {
                    string row = MapToRow(t);
                    writer.WriteLine(row);
                }
            }
        }

        /// <summary>
        /// Updates a TaxData object from data with a new instance.
        /// </summary>
        /// <param name="obj">The updated TaxData</param>
        public void Update(TaxData obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].State.ToLower().Equals(obj.State.ToLower()))
                    Data[i] = obj;
            Save();
        }

    }
}
