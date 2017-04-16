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
        public ObservableCollection<Series> SearchForSeries(string _seriesName, AddSeriesViewModel main)
        {
            ObservableCollection<Series> _series = new ObservableCollection<Series>();

            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/search/series?name=" + _seriesName);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.tvdb_token);
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    JsonTextReader reader = new JsonTextReader(new StringReader(result));
                    int _id = 0;
                    string _title = "", _overview = "";
                    while (reader.Read())
                    {
                        if (reader.Value != null && reader.Value.ToString().Equals("id"))
                        {
                            do  { reader.Read(); }
                            while (reader.Value == null);

                            _id = Convert.ToInt32(reader.Value);
                        }
                        else if(reader.Value != null && reader.Value.ToString().Equals("seriesName"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _title = reader.Value.ToString();
                        }
                        else if (reader.Value != null && reader.Value.ToString().Equals("overview"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _overview = reader.Value.ToString();
                        }

                        if (_overview != "" && _title != "" && _id != 0)
                        {
                            Series s = new Series();
                            s._title = _title;
                            s._description = _overview;
                            s._id = _id;
                            s._memoryStream = SearchForPosters(_id);
                            _series.Add(s);

                            _id = 0;
                            _title = "";
                            _overview = "";
                            _found++;

                            if (main.Abort)
                            {
                                main.Abort = false;
                                return _series;
                            }
                            main.Found = _found;
                        }
                    }
                }


                return _series;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        private byte[] SearchForPosters(int _id)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/series/" + _id + "/images/query?keyType=poster");
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.tvdb_token);
            try
            {
                string _url = "";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    JsonTextReader reader = new JsonTextReader(new StringReader(result));

                    while (reader.Read())
                    {
                        if (reader.Value != null && reader.Value.ToString().Equals("fileName"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _url = reader.Value.ToString();
                        }
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
            catch (Exception e)
            {
                return null;
            }
        }

        public void SearchForAllPosters(int _id, AddSeriesViewModel main)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/series/" + _id + "/images/query?keyType=poster");
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.tvdb_token);
            ObservableCollection<Poster> _PosterList = new ObservableCollection<Poster>();
            try
            {
                
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    JsonTextReader reader = new JsonTextReader(new StringReader(result));
                    
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
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => main.PosterList.Add(new Poster(array))));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                ;
            }
        }



        public void getMoreSeriesInfo(int _id, AddSeriesViewModel main)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/series/" + _id + "/episodes/summary");
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.tvdb_token);
            ObservableCollection<Poster> _PosterList = new ObservableCollection<Poster>();
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    JsonTextReader reader = new JsonTextReader(new StringReader(result));
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
            }
            catch (Exception e)
            {
                ;
            }
        }

        
    }
}
