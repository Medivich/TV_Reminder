using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.View;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class ShowSelectedSeries : MotherCommand
    {
        public TrackedViewModel main;

        public ShowSelectedSeries(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("ShowSelectedSeries command");
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
            main.SeriesListVisibility = Visibility.Hidden;
            main.Description = new TV_Reminder.View.SeriesDescription(main.selectedSeries._id);
        }
    }
}
