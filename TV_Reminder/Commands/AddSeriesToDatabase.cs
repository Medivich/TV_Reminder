﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class AddSeriesToDatabase : ICommand
    {
        private readonly AddSeriesViewModel main;

        public AddSeriesToDatabase(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("AddToDatabase command");
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
            if (main.SeriesInfo == System.Windows.Visibility.Visible)
                return true;
            else
                return false;
        }

        public void Execute(object parameter)
        {
            main.LoadingScreen = Visibility.Visible;
            

            Thread thr = new Thread(getToken);
            thr.IsBackground = true;
            thr.Start();

            
            
            
        }

        void getToken()
        {
            SearchTvdb S = new SearchTvdb();
            //if(NIE ISTNIEJE W BAZIE)

            //main.SelectedSeries._episode_number = 1;
            S.getAllEpisodes(main.SelectedSeries._id, 2);
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }
    }
}
