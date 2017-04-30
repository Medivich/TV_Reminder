using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class DownloadEpisodes
    {
        private Object thisLock = new Object();
        private List<Episode> ep = new List<Episode>();

        //Pobiera asynchronicznie  wszystkie odcinki i zwraca ich liste
        public List<Episode> getEpisodes(int seriesID)
        {
            List<Thread> th = new List<Thread>();
            SearchTvdb STV = new SearchTvdb();

            //Na stronie mieście się max 100 odcinków, jeden wątek zajmie się jedną odpowiedzią
            for (int i = 0; i < Math.Ceiling((decimal)STV.getOverallEpisodesNumber(seriesID) / 100); i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(addEpisodes));
                th.Add(t);
                t.IsBackground = true;
                t.Start(new ThreadParam(seriesID, i + 1));
            }

            foreach (Thread t in th)
                t.Join();

            //Po dodaniu przez wątki list - sortowanie
            ep = ep.OrderBy(x => x._seasonNumber).ThenBy(y => y._episodeNumber).ToList();

            if (ep.Count > 0)
                deleteSpecials();

            return ep;
        }

        //Usuwa odcinki specjalne
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

        //Wszystkie wątki dodają swoje pobrane odcinki do wspólnej listy
        public void sumEpisodes(List<Episode> input)
        {
            lock (thisLock)
            {
                ep.AddRange(input);
            }
        }

        //Dodaj odcinek do wspolnej listy
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
