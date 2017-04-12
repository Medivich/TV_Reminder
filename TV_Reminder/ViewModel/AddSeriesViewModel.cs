using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TV_Reminder.Commands;
using TV_Reminder.Model;

namespace TV_Reminder.ViewModel
{
    class AddSeriesViewModel : SeriesViewModel
    {
        ObservableCollection<Series> _Series = new ObservableCollection<Series>();
        string _search;

        public string Search 
        { 
            set
            {
                _search = value;
                Debug.WriteLine(value);
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
            }
        }

        public AutoCompleteFilterPredicate<object> PersonFilter
        {
            get
            {
                return (searchText, obj) =>
                    (obj as Series)._title.Contains(searchText);
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
