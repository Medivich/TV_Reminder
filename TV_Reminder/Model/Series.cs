using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TV_Reminder.Model
{
    class Series
    {
        public string _description { get; set; }

        public string _title { get; set; }

        public int _season_number { get; set; }

        public int _episode_number { get; set; }

        public int _id { get; set; }

        public bool _watched { get; set; }

        public byte[] _memoryStream { get; set; }

        public Series(string title, string description, int id, byte[] memoryStream)
        {
            this._id = id;
            this._title = title;
            this._memoryStream = memoryStream;
            this._description = description;
        }
    }
}
