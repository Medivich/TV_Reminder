using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using System.Threading;

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
            return true;
        }

        public void Execute(object parameter)
        {
            main.content = new Loading();
            // Start a thread that calls a parameterized instance method.

            Thread thr = new Thread(doSomething);
            thr.SetApartmentState(ApartmentState.STA);
            thr.Start();
        }

        void doSomething()
        {
            Thread.Sleep(2500);
            main.content.Dispatcher.Invoke(new Action(() => main.content = new AddSeries()));        
        }
    }
}
