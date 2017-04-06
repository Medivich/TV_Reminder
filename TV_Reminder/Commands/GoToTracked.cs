using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using System.Windows.Input;

namespace TV_Reminder.Commands
{
    class GoToTracked : ICommand
    {
        private readonly MainViewModel main;

        public GoToTracked(MainViewModel main)
        {
            if (main == null) throw new ArgumentNullException("Go To Tracked Command");
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
            main.content = new Tracked();
        }
    }
}
