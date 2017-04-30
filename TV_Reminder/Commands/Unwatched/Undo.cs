using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.Unwatched
{
    class Undo : MotherCommand
    {
        private readonly UnwatchedViewModel main;
        public Undo(UnwatchedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            ;
        }
    }
}
