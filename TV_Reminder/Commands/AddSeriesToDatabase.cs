using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            SearchTvdb S = new SearchTvdb();
            S.getAllEpisodes(main.SelectedSeries._id);
        }
    }
}
