using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using Newtonsoft.Json;

namespace TV_Reminder.Control
{
    class LogToTvdb
    {
        private string apikey = "0C8F86B9CBFC4A56";
        private string userkey = "D9D5E7CBC06279A4";
        private string username = "Medivich";
        //Pobieranie tokenu autoryzacyjnego z bazy tvdb
        //Jak się uda - zwraca token, jak nie - zwraca null
        public string GetToken()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://api.thetvdb.com/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json =
                 "{\"apikey\": \"" + apikey + "\",\n" +
                      "\"userkey\": \"" + userkey + "\",\n" +
                      "\"username\": \"" + username + "\"}";
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    JsonTextReader reader = new JsonTextReader(new StringReader(result));

                    while(reader.Read())
                        if(reader.Value != null)
                        {                 
                            if(reader.ReadAsString().Equals("token"))
                                reader.Read();

                            return reader.Value.ToString();
                        }
                    MessageBox.Show("Zła reakcja na zapytanie", "Error");
                    return null;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }
    }
}
