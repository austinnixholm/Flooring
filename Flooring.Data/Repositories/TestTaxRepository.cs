using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flooring.Models;
using Flooring.Models.Interfaces;

namespace Flooring.Data.Repositories
{
    /// <summary>
    /// Represents a test TaxData repository to be used in Unit tests
    /// </summary>
    public class TestTaxRepository : ITaxRepository
    {
        public List<TaxData> Data { get; set; }

        public TestTaxRepository()
        {
            Data = new List<TaxData>()
            {
                new TaxData()
                {
                    State = "Ohio",
                    StateAbbreviation = "OH",
                    TaxRate = 6.25M
                }
            };
        }

        public List<TaxData> GetAll()
        {
            return Data;
        }

        public List<TaxData> GetAllByValue(object value)
        {
            return !(value is string) ? null : Data.Where(t => t.State.ToLower() == ((string)value).ToLower()).ToList();
        }

        public TaxData GetByValue(object value)
        {
            return !(value is string) ? null : Data.FirstOrDefault(t => t.State.ToLower() == ((string)value).ToLower());
        }

        public void Add(TaxData obj)
        {
            if (Data.Any(t => t.StateAbbreviation.ToLower() == obj.State.ToLower())) return;
            Data.Add(obj);
        }

        public void Update(TaxData obj)
        {
            for (int i = 0; i < Data.Count; i++)
                if (Data[i].State.ToLower().Equals(obj.State.ToLower()))
                    Data[i] = obj;
        }

        public void Delete(object value)
        {
            if (!(value is string)) return;
            Data.Remove(Data.FirstOrDefault(t => t.State.ToLower() == ((string)value).ToLower()));
        }

        public void Save()
        {
            //Test repos shouldn't save
            throw new NotImplementedException();
        }
    }
}
