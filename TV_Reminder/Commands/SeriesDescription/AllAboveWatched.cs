using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.SeriesDescription
{
    class AllAboveWatched : MotherCommand
    {
        private readonly SeriesDescriptionViewModel main;

        public AllAboveWatched(SeriesDescriptionViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            UpdateDataBase UBD = new UpdateDataBase();
            UBD.AllAboveWatched(main.seriesId, main.SelectedEpisode._episodeNumber, main.SelectedEpisode._seasonNumber, true);
            main.UpdateTree();
        }
    }
}
