using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.SeriesDescription
{
    class ChangeBanner : MotherCommand
    {
        private readonly SeriesDescriptionViewModel main;
        public ChangeBanner(SeriesDescriptionViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.PickBanner = System.Windows.Visibility.Visible;
            main.BannerList.Add(main.SelectedSeries._banner);

            Thread t = new Thread(addPosters);
            main.UpdatePosterList();
            t.IsBackground = true;
            t.Start();
        }

        public void addPosters()
        {
            new SearchTvdb().SearchForAllBanners(main.seriesId, main);
        }
    }
}
