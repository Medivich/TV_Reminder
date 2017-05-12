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
        public static string connString = @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename="
                            + Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) +
                            @"\Other\DataBase\Tv_Reminder_Database.mdf;Integrated Security=True";

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
