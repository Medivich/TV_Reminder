using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TV_Reminder.Model
{
    class Token
    {
        public static string tvdb_token;

        public string token
        {
            get { return tvdb_token; }
            set
            {
                tvdb_token = value;
            }
        }
    }
}
