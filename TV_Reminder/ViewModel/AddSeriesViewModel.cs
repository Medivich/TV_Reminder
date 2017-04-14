using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
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
        Visibility _searchList = Visibility.Hidden;

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
                _Series = value;

                OnPropertyChanged("Series");
                OnPropertyChanged("Search");
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
