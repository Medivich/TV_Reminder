using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class UpdateSelectedSeries : MotherCommand
    {
        private readonly TrackedViewModel main;
        public UpdateSelectedSeries(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("UpdateSelectedSeries command");
            this.main = main;
        }

        public override bool CanExecute(object parameter)
        {
            if (main.selectedSeries != null)
                return true;
            else
                return false;
        }

        override public void Execute(object parameter)
        {
            ;
        }
    }
}
