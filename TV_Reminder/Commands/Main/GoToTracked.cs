using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using TV_Reminder.Control;
using System.Windows.Input;
using System.Threading;
using System.Collections.ObjectModel;
using TV_Reminder.Model;

namespace TV_Reminder.Commands
{
    class GoToTracked : MotherCommand
    {
        private readonly MainViewModel main;

        public GoToTracked(MainViewModel main)
        {
            if (main == null) throw new ArgumentNullException("Go To Tracked Command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.content = new Loading();

            Thread thr = new Thread(getToken);
            thr.IsBackground = true;
            thr.Start();
        }

        void getToken()
        {
            ReadFromDataBase RD = new ReadFromDataBase();

            if (RD.DatabaseConnected())
                main.content.Dispatcher.Invoke(new Action(() => main.content = new View.Tracked()));
            else
                main.content.Dispatcher.Invoke(new Action(() => main.content = new Hello())); 
        }
    }
}
