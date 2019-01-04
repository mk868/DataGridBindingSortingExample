using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridBindingSortingExample.Models
{
    public class DataModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public DataModel()
        {
        }

        public DataModel(int id, string name, int count, decimal price)
        {
            this.Id = id;
            this.Name = name;
            this.Count = count;
            this.Price = price;
        }
    }
}
