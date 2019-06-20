using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data
{
    /// <summary>
    /// The order repository class that pulls data from the file system.
    /// </summary>
    public class OrderRepository : IOrderRespository
    {
        public readonly string FileHeader = FileManager.OrderFileHeader;
        public readonly string FilePath = FileManager.OrdersPath;
        public readonly string Separator = FileManager.Separator;

        public string OrderDate { get; set; }
        public List<Order> Data { get; set; }

        public OrderRepository(string orderDate)
        {
            OrderDate = orderDate.Replace("/", "");
            Data = GetAll();
        }

        /// <summary>
        /// Maps each line of the file within the file path to an order object & compiles a list.
        /// </summary>
        /// <returns>List of orders from the file</returns>
        public List<Order> GetAll()
        {
            List<Order> orders = new List<Order>();
            DateTime date = GetDate();

            string path = FileManager.GetFilePath(FilePath, date);
            if (File.Exists(path))
            {
                using (StreamReader reader = new StreamReader(path, true))
                {
                    reader.ReadLine(); //skip header
                    string line = string.Empty;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.ToLower().Equals(FileHeader.ToLower())) continue;
                        Order order = MapToData(line);
                        if (order != null)
                            orders.Add(order);
                    }
                }
            }
            else
            {
                CreateFileWithHeader(path);
            }
            return orders;
        }

        /// <summary>
        /// Opens a streamwriter to create a file (if it doesn't already exist within the path)
        /// Writes the header at the top of the txt file
        /// </summary>
        /// <param name="path">The file path</param>
        private void CreateFileWithHeader(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(FileHeader);
            }
        }

        /// <summary>
        /// Gets a list of all Order objects who's customer name is equal to the parameter
        /// </summary>
        /// <param name="value">The customer name to check</param>
        /// <returns>A list of found orders</returns>
        public List<Order> GetAllByValue(object value)
        {
            return !(value is string)
                ? null
                : Data.Where(o => o.CustomerName.ToLower().Equals(((string)value).ToLower())).ToList();
        }

        /// <summary>
        /// Gets an order object from the data list by Order Number
        /// </summary>
        /// <param name="value">The order number</param>
        /// <returns>Order from data if it exists</returns>
        public Order GetByValue(object value)
        {
            return !(value is int) ? null : Data.FirstOrDefault(o => o.OrderNumber.Equals((int)value));
        }

        /// <summary>
        /// Adds a new order object to the Data list, then saves the list to the file.
        /// </summary>
        /// <param name="obj">The order to add</param>
        public void Add(Order obj)
        {
            if (Data.Any(o => o.OrderNumber == obj.OrderNumber)) return;
            Data.Add(obj);
            Save();
        }

        /// <summary>
        /// Updates an order object within the Data list with a new order
        /// </summary>
        /// <param name="obj">The updated Order object</param>
        public void Update(Order obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].OrderNumber.Equals(obj.OrderNumber))
                    Data[i] = obj;
            Save();
        }

        /// <summary>
        /// Removes an order from the data list, and if the file contains no more data, delete the file.
        /// </summary>
        /// <param name="value">The ordernumber of the order to delete</param>
        public void Delete(object value)
        {
            if (!(value is int)) return;
            Data.Remove(Data.FirstOrDefault(o => o.OrderNumber.Equals((int)value)));
            if (Data.Count > 0)
                Save();
            else
                FileManager.DeleteFile(FilePath, GetDate());
        }

        /// <summary>
        /// Saves all Order data to the file.
        /// </summary>
        public void Save()
        {
            DateTime date = GetDate();

            if (date == default(DateTime)) return;

            string path = FileManager.GetFilePath(FilePath, date);
            if (!File.Exists(path))
                return;
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(FileHeader);
                foreach (Order o in Data)
                {
                    string row = MapToRow(o);
                    writer.WriteLine(row);
                }
            }
        }

        /// <summary>
        /// Creates an order object based on data from a line within the file.
        /// </summary>
        /// <param name="s">The read line</param>
        /// <returns>An order object with the found data</returns>
        private Order MapToData(string s)
        {
            string[] split = s.Split(Convert.ToChar(Separator));
            if (split.Length != 12) return null;

            Order data = new Order();

            data.OrderNumber = int.Parse(split[0]);
            data.CustomerName = split[1];
            data.TaxData = new TaxData();
            data.TaxData.State = split[2];
            data.TaxData.TaxRate = decimal.Parse(split[3]);
            data.Product = new Product();
            data.Product.ProductType = split[4];
            data.Area = decimal.Parse(split[5]);
            data.Product.CostPerSquareFoot = decimal.Parse(split[6]);
            data.Product.LaborCostPerSquareFoot = decimal.Parse(split[7]);
            data.MaterialCost = decimal.Parse(split[8]);
            data.LaborCost = decimal.Parse(split[9]);
            data.Tax = decimal.Parse(split[10]);
            data.Total = decimal.Parse(split[11]);

            return data;
        }

        /// <summary>
        /// Converts an order object to a string that can be written to file.
        /// </summary>
        /// <param name="data">The Order object</param>
        /// <returns>A parseable string</returns>
        private string MapToRow(Order data)
        {
            string row = string.Empty;

            row += data.OrderNumber + Separator;
            row += data.CustomerName + Separator;
            row += data.TaxData.State + Separator;
            row += data.TaxData.TaxRate + Separator;
            row += data.Product.ProductType + Separator;
            row += data.Area + Separator;
            row += data.Product.CostPerSquareFoot + Separator;
            row += data.Product.LaborCostPerSquareFoot + Separator;
            row += data.MaterialCost + Separator;
            row += data.LaborCost + Separator;
            row += data.Tax + Separator;
            row += data.Total;
            return row;
        }

        /// <summary>
        /// Creates a DateTime object from this instance's orderdate string.
        /// </summary>
        /// <returns>A parsed DateTime object, or default</returns>
        private DateTime GetDate()
        {
            DateTime date = new DateTime();
            try
            {
                date = DateTime.ParseExact(OrderDate, "MMddyyyy", CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
            }
            return date;
        }
    }
}