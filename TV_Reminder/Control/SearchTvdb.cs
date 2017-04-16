using Newtonsoft.Json;
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
    class SearchTvdb
    {
        int _found = 0;

        //Zwraca listę seriali, a dokładnie ich: id, najnowszy plakat jako byte[], tytuł, opis
        public ObservableCollection<Series> SearchForSeries(string _seriesName, AddSeriesViewModel main)
        {
            getJSONreply json = new getJSONreply();
            JsonTextReader reader = json.getReply("https://api.thetvdb.com/search/series?name=" + _seriesName);

            try
            {
                int _id = 0;
                string _title = "", _overview = "";
                ObservableCollection<Series> _series = new ObservableCollection<Series>();

                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        if (reader.Value.ToString().Equals("id"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _id = Convert.ToInt32(reader.Value);
                        }
                        else if (reader.Value.ToString().Equals("seriesName"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _title = reader.Value.ToString();
                        }
                        else if (reader.Value.ToString().Equals("overview"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _overview = reader.Value.ToString();
                        }
                    }

                    if (_overview != "" && _title != "" && _id != 0)
                    {
                        _series.Add(new Series(_title, _overview, _id, SearchForPosters(_id)));

                        _id = 0;
                        _title = "";
                        _overview = "";
                        _found++;

                        if (main.AbortSearch) // Jak naciśnięty przycisk AbortSearch
                        {
                            main.AbortSearch = false;
                            return _series;
                        }
                        main.FoundSeries = _found;
                    }
                }
                return _series;         
            }
            catch (Exception e)
            {
                MessageBox.Show("Nie znaleziono takiego serialu", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                return null;
            }
        }

        //Zwraca najnowszy plakat
        private byte[] SearchForPosters(int _seriesID)
        {
            getJSONreply json = new getJSONreply();
            JsonTextReader reader = json.getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");
            string _url = "";

            if (reader != null)
            {
                while (reader.Read())
                {
                    if (reader.Value != null && reader.Value.ToString().Equals("fileName"))
                    {
                        do { reader.Read(); }
                        while (reader.Value == null);

                        _url = reader.Value.ToString();
                    }
                }

                string fullUrl = "http://thetvdb.com/banners/" + _url;
                byte[] array;

                using (WebClient client = new WebClient())
                {
                    array = client.DownloadData(new Uri(fullUrl));
                }
                return array;
            }
            else
                return null;
        }

        //Wyszukuje plakaty i zwraca je po kolei do _PosterList
        public void SearchForAllPosters(int _seriesID, AddSeriesViewModel main)
        {    
            getJSONreply json = new getJSONreply();
            JsonTextReader reader = json.getReply("https://api.thetvdb.com/series/" + _seriesID + "/images/query?keyType=poster");
            if(reader != null)
                while (reader.Read())
                {
                    if (reader.Value != null && reader.Value.ToString().Equals("fileName"))
                    {
                        do { reader.Read(); }
                        while (reader.Value == null);

                        string _url = "http://thetvdb.com/banners/" + reader.Value.ToString();
                        byte[] array;

                        using (WebClient client = new WebClient())
                        {
                            array = client.DownloadData(new Uri(_url));
                        }
                        Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                            main.PosterList.Add(new Poster(array))));
                    }
                }
        }

        //Zwraca ilosc odcinkow
        public void getOverallEpisodesNumber(int _seriesID, AddSeriesViewModel main)
        {
            getJSONreply json = new getJSONreply();
            JsonTextReader reader = json.getReply("https://api.thetvdb.com/series/" + _seriesID + "/episodes/summary");
            while (reader.Read())
            {
                if (reader.Value != null && reader.Value.ToString().Equals("airedEpisodes"))
                {
                    do { reader.Read(); }
                    while (reader.Value == null);

                    int p;
                    Int32.TryParse(reader.Value.ToString(), out p);
                    Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(()
                        => main.EpisodeNumber = p));
                }
            }
        }

        public void getAllEpisodes(int _seriesID)
        {
            getJSONreply json = new getJSONreply();
            JsonTextReader reader = json.getReply("https://api.thetvdb.com/series/" + _seriesID + "/episodes");
            Debug.WriteLine("BIORE SIE ZA CZYTANIE");
            while(reader.Read())
            {
                if (reader.Value != null)
                    Debug.WriteLine("{0} : {1}", reader.TokenType, reader.Value);
            }
        }
    }
}
