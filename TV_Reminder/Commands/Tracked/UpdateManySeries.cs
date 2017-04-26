using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class UpdateManySeries : MotherCommand
    {
        private readonly TrackedViewModel main;
        public UpdateManySeries(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("UpdateManySeries command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            ;
        }
    }
}
