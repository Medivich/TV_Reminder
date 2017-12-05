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
using System.Threading;
using System.Windows.Threading;

namespace TV_Reminder.Commands
{
    class SearchSeries : MotherCommand
    {
        private readonly AddSeriesViewModel main;
        public SearchSeries(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("SearchSeries command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.SearchingScreen = Visibility.Visible;
            main.LoadingScreen = Visibility.Visible;
            main.ReplyList = Visibility.Hidden;
            main.FoundSeries = 0;
            // Zaczyna nowy wątek, żeby w międzyczasie UI było aktualizowane
            Thread t = new Thread(search);
            t.IsBackground = true;
            t.Start();        
        }

        void search()
        {
            ObservableCollection<Series> _Series = new SearchTvdb().SearchForSeries(main.SearchQuery, main);
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,new Action(() => main.Series = _Series));
        }
    }
}
