using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class AddSeriesToDatabase : MotherCommand
    {
        private readonly AddSeriesViewModel main;
        private Object thisLock = new Object();
        private List<Episode> ep = new List<Episode>();

        public AddSeriesToDatabase(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("AddToDatabase command");
            this.main = main;
        }

        //Czy kontrolka jest aktywna
        override public bool CanExecute(object parameter)
        {
            if (main.SeriesInfo == System.Windows.Visibility.Visible && !main.getExist())
                return true;
            else
                return false;
        }

        override public void Execute(object parameter)
        {
            main.LoadingScreen = Visibility.Visible;

            Thread t0 = new Thread(add);
            t0.IsBackground = true;
            t0.Start();   
        }

        //Tworzy wątki, które pobierają odcinki (w jednej paczce przechodzi max. 100 odcinków)
        private void add()
        {
            List<Thread> th = new List<Thread>();

            for (int i = 0; i < Math.Ceiling((decimal)main.EpisodeNumber / 100); i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(addEpisodes));
                th.Add(t);
                t.IsBackground = true;
                t.Start(new ThreadParam(main.SelectedSeries._id, i + 1));
            }

            foreach (Thread t in th)
                t.Join();

            //Po dodaniu przez wątki list - sortowanie
            ep = ep.OrderBy(x => x._seasonNumber).ThenBy(y => y._episodeNumber).ToList();

            if(ep.Count > 0)
                deleteSpecials();
     
            try
            {
                AddToDataBase add = new AddToDataBase();
                if(main.SelectedPoster != null)
                    add.addTvSeries(main.SelectedSeries._seriesName, main.SelectedSeries._id, main.SelectedSeries._overview, main.SelectedPoster._memoryStream);
                else
                    add.addTvSeries(main.SelectedSeries._seriesName, main.SelectedSeries._id, main.SelectedSeries._overview);

                //{0001-01-01 00:00:00}
                foreach(Episode e in ep)
                {
                    try
                    {
                        add.addEpisodes(main.SelectedSeries._id, e._seasonNumber, e._episodeNumber, e._episodeName, e._id, e._overview,
                            e._lastUpdate, e._aired);
                    }
                    catch(Exception x)
                    {
                        add.addEpisodes(main.SelectedSeries._id, e._seasonNumber, e._episodeNumber, e._episodeName, e._id, e._overview,
                            e._lastUpdate);
                    }   
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Application.Current.Dispatcher.Invoke(new Action(() => main.setExist(true)));
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }

        private void deleteSpecials()
        {
          
            if (ep[0]._seasonNumber == 0)
            {
                int j = 0;
                bool cont = true;
                while (cont)
                {
                    if (ep[j]._seasonNumber != 0)
                    {
                        cont = false;
                    }
                    else
                        j++;
                }

                ep.RemoveRange(0, j);
            }
        }
 

        public void sumEpisodes(List<Episode> input)
        {
            lock (thisLock)
            {
                ep.AddRange(input);
            }
        }


        private void addEpisodes(object threadParam)
        {
            ThreadParam context = (ThreadParam)threadParam;
            SearchTvdb S = new SearchTvdb();          
            sumEpisodes(S.getAllEpisodes(context._id, context._page));
        }

        class ThreadParam
        {
            public int _id;
            public int _page;

            public ThreadParam(int id, int page)
            {
                this._id = id;
                this._page = page;
            }
        }

    }
}
