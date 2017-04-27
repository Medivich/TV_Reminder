using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Control
{
    class getJSONreply
    {
        public string getReply(string uri)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Accept = "application/json";
            httpWebRequest.Headers.Add("Authorization", "Bearer " + Model.Token.token);

            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                StreamReader s = new StreamReader(httpResponse.GetResponseStream());
                return s.ReadToEnd();
            }
            catch (Exception e)
            {
                ;
            }
            return null;
        }
    }
}
