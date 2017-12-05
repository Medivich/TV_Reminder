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
            new GoogleSearch().findInGoogle(main.SelectedWrapper._seriesName + " S" + main.SelectedWrapper._episode.SeasonNumber + "E" + main.SelectedWrapper._episode.EpisodeNumber);
            
            //Oznacza epizod jak obejrzany
            new UpdateDataBase().SetWatched(main.SelectedWrapper._episode._id, true);
            //Usuwa epizod z listy
            main.WrapperList.Remove(main.SelectedWrapper);

            //Dodaje kolejny epizod
            if (!main.ShowAll)
            {
                Wrapper w = new Wrapper(new ReadFromDataBase().GetLastAvaiableEpisode(main.SelectedWrapper._seriesID),
                    main.SelectedWrapper._banner,
                    main.SelectedWrapper._seriesName, main.SelectedWrapper.ShowBanner,
                    main.SelectedWrapper._rating, main.SelectedWrapper._seriesID);

                if (w._episode != null)
                {
                    main.WrapperList.Add(w);
                    main._lastAdded = w;
                }
            }
            //Zapisuje usuniety epizod, oznacza, co zostało dodane (zeby móc cofnąć akcję)
            main._selected = false;
            main._undo = true;
            main._undoWrapper = main.SelectedWrapper;
            main.reloadWrapperList();
        }
    }
}
