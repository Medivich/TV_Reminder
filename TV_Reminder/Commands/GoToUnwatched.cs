using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using System.Windows.Input;

namespace TV_Reminder.Commands
{
    class GoToUnwatched : MotherCommand
    {
        //To zmienić jak powstanie viewmodel
        private readonly MainViewModel main;

        public GoToUnwatched(MainViewModel main)
        {
            if (main == null) throw new ArgumentNullException("Go To unwatched Command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.content = new Unwatched();
        }
    }
}
