using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.View;

namespace TV_Reminder.Commands
{

    class GoToAddSeries : ICommand
    {
        private readonly MainViewModel main;

        public GoToAddSeries(MainViewModel main)
        {
            if (main == null) throw new ArgumentNullException("Go To Series Command");
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
            return false;
        }

        public void Execute(object parameter)
        {
            main.content = new AddSeries();
        }
    }
}
