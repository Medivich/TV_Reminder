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
    class DeleteSelectedSeries : MotherCommand
    {
        private readonly TrackedViewModel main;
        public DeleteSelectedSeries(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
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
            new DeleteFromDataBase().deleteEpisodes(main.selectedSeries._id);
            new DeleteFromDataBase().deleteSeries(main.selectedSeries._id);

            Application.Current.Dispatcher.Invoke(new Action(() => main.seriesList.Remove(main.selectedSeries)));
            Application.Current.Dispatcher.Invoke(new Action(() => main.selectedSeries = null));

            string con = "";
            con += "Dodane seriale: " + new ReadFromDataBase().GetSeriesNo() + "\n";
            con += "Dodane odcinki: " + new ReadFromDataBase().GetEpisodeNo() + "\n";
            con += "Obejrzane odcinki: " + new ReadFromDataBase().GetWatchedEpisodesNo(true) + "\n";
            con += "Nieobejrzane odcinki: " + new ReadFromDataBase().GetWatchedEpisodesNo(false) + "\n";

            Application.Current.Dispatcher.Invoke(new Action(() => main.Statystyka = con));
        }
    }
}
