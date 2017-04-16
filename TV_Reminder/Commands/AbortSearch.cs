using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class AbortSearch : ICommand
    {
        private readonly AddSeriesViewModel main;

        public AbortSearch(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("AbortSearch command");
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
            main.AbortSearch = true;
        }
    }
}
