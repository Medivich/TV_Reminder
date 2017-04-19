using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Control
{
    class SearchTvdb : getJSONreply
    {
        //Zwraca wyszukane seriale, w tym ich: ID, tytuł, opis, najnowszy plakat
        public ObservableCollection<Series> SearchForSeries(string title, AddSeriesViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/search/series?name=" + title);
            ObservableCollection<Series> _series = new ObservableCollection<Series>();

            try
            {
                JObject tvdbSearch = JObject.Parse(JSON);
                IList<JToken> results = tvdbSearch["data"].Children().ToList();
                int _found = 0;

                foreach (JToken result in results)
                {                  
                    Series searchResult = result.ToObject<Series>();

                    byte[] poster;
                    if ((poster = SearchForPosters(searchResult._id)) != null)
                        searchResult._poster = poster;

                    _series.Add(searchResult);

                    if (main.AbortSearch) // Jak naciśnięty przycisk AbortSearch
                    {
                        main.AbortSearch = false;
                        return _series;
                    }
                    main.FoundSeries = ++_found;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return _series;             
        }

        //Zwraca najnowszy plakat
        private byte[] SearchForPosters(int _seriesID)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");

            if (JSON != null)
            {
                JObject tvdbSearch = JObject.Parse(JSON);
                IList<JToken> results = tvdbSearch["data"].Children().ToList();

                Poster p = new Poster();
                byte[] array = null;

                foreach (JToken result in results)
                {
                    p = result.ToObject<Poster>();
                }

                using (WebClient client = new WebClient())
                {
                    array = client.DownloadData(new Uri("http://thetvdb.com/banners/" + p.fileName));
                }
                return array;
            }
            return null;                
        }
        
        //Wyszukuje plakaty i dodaje je po kolei do _PosterList
        public void SearchForAllPosters(int _seriesID, AddSeriesViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");

            JObject tvdbSearch = JObject.Parse(JSON);
            IList<JToken> results = tvdbSearch["data"].Children().ToList();

            foreach (JToken result in results)
            {
                Poster p = result.ToObject<Poster>();

                byte[] array;
                using (WebClient client = new WebClient())
                {
                    array = client.DownloadData(new Uri("http://thetvdb.com/banners/" + p.fileName));
                }

                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    main.PosterList.Add(new Poster(array))));
            }
        }


        //Zwraca ilosc odcinkow
        public void getOverallEpisodesNumber(int _seriesID, AddSeriesViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/episodes/summary");
            JObject tvdbSearch = JObject.Parse(JSON);

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(()
                => main.EpisodeNumber = tvdbSearch["data"]["airedEpisodes"].ToObject<int>()));
        }

        public List<Episode> getAllEpisodes(int seriesID, int page)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + seriesID + "/episodes?page=" + page);

            List<Episode> _episodeList = new List<Episode>();
            try
            {
                JObject tvdbSearch = JObject.Parse(JSON);
                IList<JToken> results = tvdbSearch["data"].Children().ToList();

                foreach (JToken result in results)
                {
                    try
                    {
                        Episode searchResult = result.ToObject<Episode>();
                        _episodeList.Add(searchResult);
                    }
                    catch (Exception)
                    {
                        ;
                    }

                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return _episodeList;
        }
    }
}
