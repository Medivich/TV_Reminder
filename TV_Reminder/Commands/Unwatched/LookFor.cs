using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.Unwatched
{
    class LookFor : MotherCommand
    {
        private readonly UnwatchedViewModel main;
        public LookFor(UnwatchedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        public override bool CanExecute(object parameter)
        {
            return main._selected;
        }

        override public void Execute(object parameter)
        {
            GoogleSearch GS = new GoogleSearch();
            GS.findInGoogle(main.SelectedWrapper._seriesName + " S" + main.SelectedWrapper._episode.SeasonNumber + "E" + main.SelectedWrapper._episode.EpisodeNumber);
            
            UpdateDataBase UD = new UpdateDataBase();
            UD.SetWatched(main.SelectedWrapper._episode._id, true);

            main.WrapperList.Remove(main.SelectedWrapper);

            ReadFromDataBase RD = new ReadFromDataBase();
            if (!main.ShowAll)
            {
                Wrapper w = new Wrapper(RD.GetLastAvaiableEpisode(main.SelectedWrapper._seriesID), main.SelectedWrapper._banner,
                    main.SelectedWrapper._seriesName, main.SelectedWrapper.ShowBanner,
                    main.SelectedWrapper._rating, main.SelectedWrapper._seriesID);

                if (w._episode != null)
                {
                    main.WrapperList.Add(w);
                    main._lastAdded = w;
                }
            }

            main._selected = false;
            main._undo = true;
            main._undoWrapper = main.SelectedWrapper;
            main.reloadWrapperList();
        }
    }
}
