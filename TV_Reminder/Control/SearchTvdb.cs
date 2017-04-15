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
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class SearchTvdb
    {
        int balance = 0;
        private Object thisLock = new Object();

        private void Podnies()
        {
            lock (thisLock)
            {
                balance += 1;
            }
        }

        private void Opusc()
        {
            lock (thisLock)
            {
                balance -= 1;
            }
        }

        private int getLock()
        {
            lock (thisLock)
            {
                return balance;
            }
        }

        public ObservableCollection<Series> SearchForSeries(string _seriesName)
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
                            Debug.WriteLine(_title);
                        }
                        else if (reader.Value != null && reader.Value.ToString().Equals("overview"))
                        {
                            do { reader.Read(); }
                            while (reader.Value == null);

                            _overview = reader.Value.ToString();
                            Debug.WriteLine(_overview); 
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
                /*
                WebRequest requestPic = WebRequest.Create(_url);
                WebResponse responsePic = requestPic.GetResponse();
                Stream responseStream = responsePic.GetResponseStream();



                BitmapImage a = new BitmapImage();
                a.BeginInit();
                Podnies();
                a.CacheOption = BitmapCacheOption.OnLoad;
                a.StreamSource = responseStream;      
                a.EndInit();*/
                
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
