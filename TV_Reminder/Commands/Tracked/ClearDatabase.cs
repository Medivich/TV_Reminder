using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.Tracked
{
    class ClearDatabase : MotherCommand
    {
        private readonly TrackedViewModel main;
        public ClearDatabase(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("ClearDatabase command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Visible));

            Thread t0 = new Thread(clear);
            t0.IsBackground = true;
            t0.Start();
        }

        private void clear()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => main.clearLog()));
            
            foreach (Series s in main.seriesList)
            {
                if (s._update)
                {
                    AddToDataBase AD = new AddToDataBase();
                    List<Episode> ep = new List<Episode>();
                    Episode last = new ReadFromDataBase().GetLastAvaiableEpisode(s._id);
                    
                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Pobieram " + s._seriesName)));
                    ep = new DownloadEpisodes().getEpisodes(s._id);

                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Aktualizuję " + s._seriesName)));

                    new DeleteFromDataBase().deleteEpisodes(s._id);
                    
                    foreach (Episode e in ep)
                        AD.addEpisode(s._id, e);

                    if (last != null)
                        new UpdateDataBase().AllBelowWatched(s._id, last._episodeNumber, last._seasonNumber, true);
                    else
                        new UpdateDataBase().AllWatched(s._id, true);
                }
                else
                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Pomijam " + s._seriesName)));
            }

            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }
    }
}
