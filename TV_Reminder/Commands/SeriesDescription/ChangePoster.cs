using System;
using System.Threading;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.SeriesDescription
{
    class ChangePoster : MotherCommand
    {
        private readonly SeriesDescriptionViewModel main;

        public ChangePoster(SeriesDescriptionViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.PickPoster = System.Windows.Visibility.Visible;
            main.PosterList.Add(new Poster(main.SelectedSeries._poster));

            Thread t = new Thread(addBanners);
            main.UpdatePosterList();
            t.IsBackground = true;
            t.Start();     
        }

        public void addBanners()
        {
            new SearchTvdb().SearchForAllPosters(main.seriesId, main);
        }
    }
}
