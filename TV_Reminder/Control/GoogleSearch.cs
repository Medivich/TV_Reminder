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
        //Otwiera wyszukiwarkę i wpisuje nazwe odcinka
        public void findInGoogle(string query)
        {
            string uriString = "http://www.google.com/search?q=" + query;
            WebClient webClient = new WebClient();

            System.Diagnostics.Process.Start(uriString);
        }
    }
}
