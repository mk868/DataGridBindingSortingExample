using DataGridBindingSortingExample.Behaviors;
using DataGridBindingSortingExample.Commands;
using DataGridBindingSortingExample.Models;
using DataGridBindingSortingExample.Services;
using DataGridBindingSortingExample.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataGridBindingSortingExample.ViewModels
{
    class MainWindowViewModel : BaseViewModel
    {
        private readonly IDataService _dataService = new DataService();
        private readonly Random _random = new Random();

        public MainWindowViewModel()
        {
            OnUserSortCommand = new RelayCommand<OnSortCommandParams>(OnUserSortCommandExecuted);
            RandomSortCommand = new RelayCommand<object>(RandomSortCommandExecuted);
            UpdateData();
        }

        private void UpdateData(string sortColumn = null, ListSortDirection? direction = null)
        {
            if (sortColumn == null || !direction.HasValue)
            {
                Data = _dataService.GetAll();
            }
            else
            {
                Data = _dataService.GetAll(sortColumn, direction == ListSortDirection.Ascending);
            }
        }

        private void OnUserSortCommandExecuted(OnSortCommandParams param)
        {
            UpdateData(SortMemberPath, param.SortDirection);
        }

        private void RandomSortCommandExecuted(object param)
        {
            var direction = _random.Next() % 2 == 0 ? ListSortDirection.Ascending : ListSortDirection.Descending;
            var columns = new string[]
            {
                "Id",
                "Name",
                "Count"
            };
            var column = columns[_random.Next(columns.Length)];

            SortDirection = direction;
            SortMemberPath = column;

            UpdateData(column, direction);
        }

        #region Properties

        private IList<DataModel> _data { get; set; }
        public IList<DataModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                NotifyPropertyChanged("Data");
            }
        }

        private ListSortDirection? _sortDirection { get; set; }
        public ListSortDirection? SortDirection
        {
            get { return _sortDirection; }
            set
            {
                _sortDirection = value;
                NotifyPropertyChanged("SortDirection");
            }
        }

        private string _sortMemberPath { get; set; }
        public string SortMemberPath
        {
            get { return _sortMemberPath; }
            set
            {
                _sortMemberPath = value;
                NotifyPropertyChanged("SortMemberPath");
            }
        }

        public ICommand OnUserSortCommand { get; set; }

        public ICommand RandomSortCommand { get; set; }

        #endregion
    }
}
