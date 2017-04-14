using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TV_Reminder.Model;

namespace TV_Reminder.Control
{
    class SearchTvdb
    {

        /*
         * Schemat odbieranej paczki:
         * {
              "aliases": [
                "string"
              ],
              "banner": "string",
              "firstAired": "string",
              "id": 0,
              "network": "string",
              "overview": "string",
              "seriesName": "string",
              "status": "string"
            }
         */
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
                            _series.Add(new Series(_title, _overview, _id, dodajObrazek()));
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

        private ImageSource dodajObrazek()
        {
            BitmapImage a = new BitmapImage(new Uri(@"C:\Users\user\Source\Repos\TV_Reminder\TV_Reminder\Other\Image\loading.png"));
            var bitmap = new TransformedBitmap(a,
                        new ScaleTransform(
                            200 / a.Width,
                            300 / a.Height));

            ImageSource _image = bitmap;
            return _image;
        }
    }
}
