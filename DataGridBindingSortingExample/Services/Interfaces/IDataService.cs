using DataGridBindingSortingExample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataGridBindingSortingExample.Services.Interfaces
{
    public interface IDataService
    {
        IList<DataModel> GetAll();
        IList<DataModel> GetAll(string sortColumn, bool asc);
    }
}
