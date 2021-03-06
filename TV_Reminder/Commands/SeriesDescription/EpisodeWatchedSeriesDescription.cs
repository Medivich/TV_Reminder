﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class EpisodeWatchedSeriesDescription : MotherCommand
    {
        private readonly SeriesDescriptionViewModel main;
        public EpisodeWatchedSeriesDescription(SeriesDescriptionViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            if (main.SelectedEpisode != null)
            {
                if(main.SelectedEpisode._watched)
                    new UpdateDataBase().SetWatched(main.SelectedEpisode._id, false);
                else
                    new UpdateDataBase().SetWatched(main.SelectedEpisode._id, true);
                main.UpdateTree();
            }
        }
    }
}
