using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DataGridBindingSortingExample.Behaviors
{
    public class OnSortCommandParams
    {
        public ListSortDirection SortDirection { get; set; }
        public string SortMemberPath { get; set; }

        public OnSortCommandParams()
        {
        }
    }

    public class BindingSortBehavior : Behavior<DataGrid>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Sorting += HandleDataGridSorting;

            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.AddValueChanged(AssociatedObject, OnItemsSourcePropertyChanged);
            }
        }

        protected override void OnDetaching()
        {
            AssociatedObject.Sorting -= HandleDataGridSorting;

            var dpd = DependencyPropertyDescriptor.FromProperty(ItemsControl.ItemsSourceProperty, typeof(DataGrid));
            if (dpd != null)
            {
                dpd.RemoveValueChanged(AssociatedObject, OnItemsSourcePropertyChanged);
            }

            base.OnDetaching();
        }

        //prevent clearing the sort settings after changing ItemsSource
        private void OnItemsSourcePropertyChanged(object sender, EventArgs e)
        {
            var direction = this.SortDirection;
            var currentColumn = this.SortMemberPath;

            foreach (var column in AssociatedObject.Columns)
            {
                if (column.SortMemberPath == currentColumn)
                {
                    column.SortDirection = direction;
                }
            }
        }

        #region OnUserSortCommand
        public static readonly DependencyProperty OnUserSortCommandProperty =
                    DependencyProperty.RegisterAttached("OnUserSortCommand", typeof(ICommand),
                    typeof(BindingSortBehavior));

        public ICommand OnUserSortCommand
        {
            get { return (ICommand)GetValue(OnUserSortCommandProperty); }
            set { SetValue(OnUserSortCommandProperty, value); }
        }
        #endregion

        #region SortDirection
        public static readonly DependencyProperty SortDirectionProperty =
                    DependencyProperty.RegisterAttached("SortDirection", typeof(ListSortDirection?),
                    typeof(BindingSortBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SortDirectionChanged)));

        public ListSortDirection? SortDirection
        {
            get { return (ListSortDirection?)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }

        private static void SortDirectionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as BindingSortBehavior;
            var currentColumn = me.SortMemberPath;
            var newValue = (ListSortDirection?)e.NewValue;

            foreach (var column in me.AssociatedObject.Columns)
            {
                if (column.SortMemberPath == currentColumn)
                {
                    column.SortDirection = newValue;
                }
            }
        }
        #endregion

        #region SortMemberPath
        public static readonly DependencyProperty SortMemberPathProperty =
                    DependencyProperty.RegisterAttached("SortMemberPath", typeof(string),
                    typeof(BindingSortBehavior), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(SortMemberPathChanged)));


        public string SortMemberPath
        {
            get { return (string)GetValue(SortMemberPathProperty); }
            set { SetValue(SortMemberPathProperty, value); }
        }

        private static void SortMemberPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var me = d as BindingSortBehavior;
            var newValue = (string)e.NewValue;

            foreach (var column in me.AssociatedObject.Columns)
            {
                if (column.SortMemberPath == newValue)
                {
                    column.SortDirection = me.SortDirection;
                }
                else
                {
                    column.SortDirection = null;
                }
            }
        }
        #endregion


        private void HandleDataGridSorting(object sender, DataGridSortingEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid == null)
            {
                return;
            }

            var direction = (e.Column.SortDirection != ListSortDirection.Ascending)
                                ? ListSortDirection.Ascending
                                : ListSortDirection.Descending;
            var sortMemberPath = e.Column.SortMemberPath;

            e.Column.SortDirection = direction;
            SortDirection = direction;
            SortMemberPath = sortMemberPath;
            var onSortCommand = OnUserSortCommand;

            e.Handled = true;

            if (onSortCommand != null)
            {
                var param = new OnSortCommandParams();
                param.SortDirection = direction;
                param.SortMemberPath = sortMemberPath;

                onSortCommand.Execute(param);
            }
        }
    }
}
