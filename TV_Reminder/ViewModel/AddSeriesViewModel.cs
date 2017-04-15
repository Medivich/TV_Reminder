using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TV_Reminder.Commands;
using TV_Reminder.Model;

namespace TV_Reminder.ViewModel
{
    class AddSeriesViewModel : SeriesViewModel
    {
        ObservableCollection<Series> _Series = new ObservableCollection<Series>();
        string _search;
        Series _selectedSeries = null;
        Visibility _searchList = Visibility.Hidden, _searching = Visibility.Hidden;

        public Series SelectedSeries
        {
            set
            {
                if (value != null)
                {
                    _selectedSeries = value;
                    Debug.WriteLine("Selected: " + _selectedSeries._title + " id: " + _selectedSeries._id);
                    _searchList = Visibility.Hidden;
                    _search = _selectedSeries._title;
                    OnPropertyChanged("SelectedSeries", "SearchList", "Search");
                }
            }
            get
            {
                return _selectedSeries;
            }
        }

        public Visibility SearchList
        {
            set
            {
                _searchList = value;
                OnPropertyChanged("SearchList");
            }
            get
            {
                return _searchList;
            }
        }

        public Visibility Searching
        {
            set
            {
                _searching = value;
                OnPropertyChanged("Searching");
            }
            get
            {
                return _searching;
            }
        }

        public string Search 
        { 
            set
            {
                _search = value;
            }
            get
            {
                return _search;
            }
        }

        public ObservableCollection<Series> Series 
        { 
            get
            {
                return _Series;
            }
            set
            {
                if (value != null)
                {
                    _Series = value;

                    SearchList = Visibility.Visible;
                    Searching = Visibility.Hidden;
                    OnPropertyChanged("Search", "Series");
                }
            }
        }

        private ICommand SearchSeries;

        public ICommand SearchButton
        {
            get
            {
                if (SearchSeries == null)
                    SearchSeries = new SearchSeries(this);
                return SearchSeries;
            }
        }
    }
}
