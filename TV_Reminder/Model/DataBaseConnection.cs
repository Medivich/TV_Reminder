using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Model
{
    class DataBaseConnection
    {
        public static string connString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename="
                            + Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) +
                            @"\Other\DataBase\Tv_Reminder_Database.mdf;Integrated Security=True;Connect Timeout=30";

        public string ConnString
        {
            get { return connString; }
            set
            {
                connString = value;
            }
        }
    }
}
