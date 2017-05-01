using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.Control;
using TV_Reminder.Model;
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
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Visible));

            Thread t0 = new Thread(getEpisodes);
            t0.IsBackground = true;
            t0.Start();
        }

        //Aktualizuje pojedyncza serie
        private void getEpisodes()
        {
            List<Episode> ep = new List<Episode>();
            ReadFromDataBase RD = new ReadFromDataBase();
            UpdateDataBase UD = new UpdateDataBase();
            AddToDataBase AD = new AddToDataBase();
            DownloadEpisodes DE = new DownloadEpisodes();

            Application.Current.Dispatcher.Invoke(new Action(() => main.clearLog()));
            Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Pobieram " + main.selectedSeries._seriesName)));

            ep = DE.getEpisodes(main.selectedSeries._id);

            Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Aktualizuję " + main.selectedSeries._seriesName)));

            int update = 0, added = 0;
            foreach (Episode e in ep)
            {
                if (RD.EpisodeExist(e._id))
                {
                    if (RD.EpisodeLastUpdate(e._id) < e._lastUpdate)
                    {
                        UD.UpdateEpisode(e);
                        update++;
                    }
                }
                else
                {
                    AD.addEpisode(main.selectedSeries._id, e);
                    added++;
                }
            }

            Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("      Zaktualizowałem " + update + " odcinków")));
            Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("      Dodałem " + added + " odcinków")));

            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }
    }
}
