using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Control
{
    class getJSONreply
    {
        public JsonTextReader getReply(string uri)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.tvdb_token);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();

                    JsonTextReader reader = new JsonTextReader(new StringReader(result));
                    return reader;
                }
            }
            catch (Exception e)
            {
                ;
            }
            return null;
        }
    }
}
