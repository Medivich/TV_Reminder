using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class UpdateManySeries : MotherCommand
    {
        private readonly TrackedViewModel main;
        public UpdateManySeries(TrackedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("UpdateManySeries command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Visible));
            
            Thread t0 = new Thread(getEpisodes);
            t0.IsBackground = true;
            t0.Start();
        }

        //Aktualizuje wszystkie zaznaczone seriale i tworzy log z procesu
        private void getEpisodes()
        {
            Application.Current.Dispatcher.Invoke(new Action(() => main.clearLog()));
            foreach (Series s in main.seriesList)
            {
                if (s._update)
                {
                    List<Episode> ep = new List<Episode>();
                    ReadFromDataBase RD = new ReadFromDataBase();
                    UpdateDataBase UD = new UpdateDataBase();
                    AddToDataBase AD = new AddToDataBase();
                    DownloadEpisodes DE = new DownloadEpisodes();
                    
                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Pobieram " + s._seriesName)));

                    ep = DE.getEpisodes(s._id);

                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Aktualizuję " + s._seriesName)));

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
                            AD.addEpisode(s._id, e);
                            added++;
                        }
                    }

                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("      Zaktualizowałem " + update + " odcinków")));
                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("      Dodałem " + added + " odcinków")));
                }
                else
                    Application.Current.Dispatcher.Invoke(new Action(() => main.addToLog("Pomijam " + s._seriesName)));
            }


            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }
    }
}
