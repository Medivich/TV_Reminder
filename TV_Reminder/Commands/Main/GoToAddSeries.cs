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

    class GoToAddSeries : MotherCommand
    {
        private readonly MainViewModel main;

        public GoToAddSeries(MainViewModel main)
        {
            if (main == null) throw new ArgumentNullException("Go To Series Command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.content = new Loading();

            // Zaczyna nowy wątek, żeby w międzyczasie UI było updatowane
            Thread thr = new Thread(getToken);
            thr.IsBackground = true;
            thr.Start();
        }

        void getToken()
        {
            if((Model.Token.tvdb_token = new LogToTvdb().GetToken()) != null)
                main.content.Dispatcher.Invoke(new Action(() => main.content = new AddSeries()));      
            else
                main.content.Dispatcher.Invoke(new Action(() => main.content = new Hello()));   
        }
    }
}
