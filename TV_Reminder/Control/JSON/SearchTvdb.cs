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
            catch (Exception)
            {
                MessageBox.Show("Nie znalazłem pasujących seriali", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
        
        //Wyszukuje plakaty i dodaje je odrazu do _PosterList
        public void SearchForAllPosters(int _seriesID, AddSeriesViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");
            if (JSON != null)
            {
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
        }

        //Wyszukuje plakaty i dodaje je odrazu do _PosterList
        public void SearchForAllPosters(int _seriesID, SeriesDescriptionViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");
            if (JSON != null)
            {
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
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        main.UpdatePosterList()));
                }
            }
        }

        //Wyszukuje banery i dodaje je odrazu do _BannerList
        public void SearchForAllBanners(int _seriesID, AddSeriesViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=series");
            if (JSON != null)
            {
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
                        main.BannerList.Add(array)));
                }
            }
        }

        //Wyszukuje banery i dodaje je odrazu do _BannerList
        public void SearchForAllBanners(int _seriesID, SeriesDescriptionViewModel main)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=series");
            if (JSON != null)
            {
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
                        main.BannerList.Add(array)));
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                        main.UpdateBannerList()));
                }
            }
        }

        //Pobiera ogólną ilość odcinków
        public int getOverallEpisodesNumber(int _seriesID)
        {
            string JSON = getReply("https://api.thetvdb.com/series/" + _seriesID + "/episodes/summary");
            if (JSON != null)
            {
                JObject tvdbSearch = JObject.Parse(JSON);
                return tvdbSearch["data"]["airedEpisodes"].ToObject<int>();
            }
            else
                return 0;
        }

        //Pobiera listę odcinków (max 100 odcinków na stronę)
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
                    catch (Exception) // Czegoś brakuje w pakiecie, więc stara się dodać to co jest 
                    {
                        Episode e = new Episode();

                        if (result["overview"].ToString().Length > 0)
                            e._overview = result["overview"].ToString();

                        if (result["episodeName"].ToString().Length > 0)
                            e._episodeName = result["episodeName"].ToString();

                        if (result["airedSeason"].ToString().Length > 0)
                            e._seasonNumber = Convert.ToInt32(result["airedSeason"].ToString());

                        if (result["id"].ToString().Length > 0)
                            e._id = Convert.ToInt32(result["id"].ToString());

                        if (result["firstAired"].ToString().Length > 0)
                            e._aired = Convert.ToDateTime(result["firstAired"].ToString());

                        if (result["lastUpdated"].ToString().Length > 0)
                            e._lastUpdate = Convert.ToInt32(result["lastUpdated"].ToString());

                        if (result["airedEpisodeNumber"].ToString().Length > 0)
                            e._episodeNumber = Convert.ToInt32(result["airedEpisodeNumber"].ToString());

                        if(e._id != 0 && e._episodeNumber != 0 && e._seasonNumber != 0)
                            _episodeList.Add(e);
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
