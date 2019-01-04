using DataGridBindingSortingExample.Models;
using DataGridBindingSortingExample.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridBindingSortingExample.Services
{
    class DataService : IDataService
    {
        private List<DataModel> _dummyData = new List<DataModel>()
        {
            new DataModel(1, "Box", 2, 1.20m),
            new DataModel(2, "Cards", 10, 5.30m),
            new DataModel(3, "Ball", 7, 15.10m),
            new DataModel(4, "Car", 3, 45.30m),
            new DataModel(5, "Rocket", 3, 25.12m),
            new DataModel(6, "Clock", 1, 12.41m),
        };

        /// <summary>
        /// Gets data from database - MOCK
        /// </summary>
        /// <returns>list of items</returns>
        public IList<DataModel> GetAll()
        {
            return _dummyData;
        }

        /// <summary>
        /// Gets data from database + sort on database side - MOCK
        /// </summary>
        /// <returns>list of items</returns>
        public IList<DataModel> GetAll(string sortColumn, bool asc)
        {
            var result = _dummyData;

            if (sortColumn == nameof(DataModel.Id))
            {
                result = result.OrderBy(o => o.Id).ToList();
            }
            if (sortColumn == nameof(DataModel.Name))
            {
                result = result.OrderBy(o => o.Name).ToList();
            }
            if (sortColumn == nameof(DataModel.Count))
            {
                result = result.OrderBy(o => o.Count).ToList();
            }
            if (sortColumn == nameof(DataModel.Price))
            {
                throw new NotSupportedException("not supported to sort by price");
            }

            if (!asc)
            {
                result.Reverse();
            }

            return result;
        }
    }
}
