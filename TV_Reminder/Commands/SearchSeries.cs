using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.Model;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TV_Reminder.Control;
using System.Collections.ObjectModel;
using System.Windows;

namespace TV_Reminder.Commands
{
    class SearchSeries : ICommand
    {
        private readonly AddSeriesViewModel main;

        public SearchSeries(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("SearchSeries command");
            this.main = main;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        //Czy kontrolka jest aktywna
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            SearchTvdb _search = new SearchTvdb();
            main.Series = _search.SearchForSeries(main.Search);
            main.SearchList = Visibility.Visible;
        }
    }
}
