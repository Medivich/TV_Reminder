using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class ChangeRating : MotherCommand
    {
        private readonly SeriesDescriptionViewModel main;
        public ChangeRating(SeriesDescriptionViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            UpdateDataBase UBD = new UpdateDataBase();
            UBD.ChangeTvSeriesRating(main.SelectedSeries._id, Convert.ToInt32(parameter));
            Application.Current.Dispatcher.Invoke(new Action(() => main.Rating = Convert.ToInt32(parameter)));
        }
    }
}
