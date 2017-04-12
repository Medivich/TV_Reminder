using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using System.Threading;
using TV_Reminder.Control;

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

            // Zaczyna nowy wątek, żeby w międzyczasie UI było updatowane
            Thread thr = new Thread(getToken);
            thr.Start();
        }



        void getToken()
        {
            LogToTvdb L = new LogToTvdb();
            if((Model.Token.tvdb_token = L.GetToken()) != null)
                main.content.Dispatcher.Invoke(new Action(() => main.content = new AddSeries()));      
            else
                main.content.Dispatcher.Invoke(new Action(() => main.content = new Hello()));   
        }
    }
}
