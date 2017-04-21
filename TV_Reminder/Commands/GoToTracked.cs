using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Reminder.ViewModel;
using TV_Reminder.View;
using System.Windows.Input;

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
            main.content = new Tracked();
        }
    }
}
