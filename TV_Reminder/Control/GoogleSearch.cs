using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Control
{
    class GoogleSearch
    {
        public void findInGoogle(string query)
        {
            string uriString = "http://www.google.com/search?q=" + query;
            WebClient webClient = new WebClient();

            System.Diagnostics.Process.Start(uriString);
        }

        public void openSite(string url)
        {
            WebClient webClient = new WebClient();
            System.Diagnostics.Process.Start(url);
        }
    }
}
