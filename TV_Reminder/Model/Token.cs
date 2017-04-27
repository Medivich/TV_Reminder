using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TV_Reminder.Control;

namespace TV_Reminder.Model
{
    class Token
    {
        public static string tvdb_token = null;

        public static string token
        {
            get 
            { 
                if(tvdb_token == null)
                {
                    LogToTvdb L = new LogToTvdb();
                    tvdb_token = L.GetToken();
                }
                return tvdb_token; 
            }
            set
            {
                tvdb_token = value;
            }
        }
    }
}
