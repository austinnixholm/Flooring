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
    /// <summary>
    /// The product repository that pulls data from the file system.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        public readonly string FileHeader = "[ProductType,CostPerSquareFoot,LaborCostPerSquareFoot]";
        public readonly string FilePath = @"C:\Users\pelic\source\repos\Flooring\Products.txt";
        public readonly string Separator = FileManager.Separator;

        public List<Product> Data { get; set; }

        public ProductRepository()
        {
            Data = GetAll();
        }

        /// <summary>
        /// Maps each line of the file within the file path to a Product object & compiles a list.
        /// </summary>
        /// <returns>List of products from the file</returns>
        public List<Product> GetAll()
        {
            List<Product> productData = new List<Product>();
            using (StreamReader reader = new StreamReader(FilePath))
            {
                string line = string.Empty;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.ToLower().Equals(FileHeader.ToLower())) continue;
                    Product prod = MapToData(line);
                    if (prod != null)
                        productData.Add(prod);
                }
            }

            return productData;
        }

        /// <summary>
        /// Queries a list of products by product type.
        /// </summary>
        /// <param name="value">The product type</param>
        /// <returns>The queried list</returns>
        public List<Product> GetAllByValue(object value)
        {
            return !(value is string) ? null : Data.Where(p => p.ProductType.ToLower() == ((string)value).ToLower()).ToList();
        }

        /// <summary>
        /// Retrieves an individual Product from the data by product type.
        /// </summary>
        /// <param name="value">The product type</param>
        /// <returns>The retrieved data</returns>
        public Product GetByValue(object value)
        {
            return !(value is string) ? null : Data.FirstOrDefault(p => p.ProductType.ToLower() == ((string)value).ToLower());
        }

        /// <summary>
        /// Splits a string into individual values and parses them into a Product object.
        /// </summary>
        /// <param name="s">The string to parse</param>
        /// <returns>A Product object based on the values</returns>
        private Product MapToData(string s)
        {
            string[] split = s.Split(Convert.ToChar(Separator));
            if (split.Length != 3) return null;

            Product data = new Product();
            data.ProductType = split[0];
            data.CostPerSquareFoot = decimal.Parse(split[1]);
            data.LaborCostPerSquareFoot = decimal.Parse(split[2]);

            return data;
        }

        /// <summary>
        /// Converts a Product object into a parseable string to be saved to data.
        /// </summary>
        /// <param name="data">The Product object to convert</param>
        /// <returns>A parseable product string</returns>
        private string MapToRow(Product data)
        {
            string row = string.Empty;
            row += data.ProductType + Separator;
            row += data.CostPerSquareFoot + Separator;
            row += data.LaborCostPerSquareFoot;

            return row;
        }

        /// <summary>
        /// Adds a product object to the data list
        /// </summary>
        /// <param name="obj">The Product to add</param>
        public void Add(Product obj)
        {
            if (Data.Any(p => p.ProductType.ToLower() == obj.ProductType.ToLower())) return;
            Data.Add(obj);
        }

        /// <summary>
        /// Deletes a value by product type from the current repository.
        /// </summary>
        /// <param name="value">The product type</param>
        public void Delete(object value)
        {
            if (!(value is string)) return;
            Data.Remove(Data.FirstOrDefault(p => p.ProductType.ToLower() == ((string)value).ToLower()));
        }

        /// <summary>
        /// Updates a Product object from data with a new instance.
        /// </summary>
        /// <param name="obj">The updated Product</param>
        public void Update(Product obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].ProductType.ToLower().Equals(obj.ProductType.ToLower()))
                    Data[i] = obj;
            Save();
        }

        /// <summary>
        /// Saves the Data list to file
        /// </summary>
        public void Save()
        {
            using (StreamWriter writer = new StreamWriter(FilePath))
            {
                writer.WriteLine(FileHeader);
                foreach (Product p in Data)
                {
                    string row = MapToRow(p);
                    writer.WriteLine(row);
                }
            }
        }
    }
}
