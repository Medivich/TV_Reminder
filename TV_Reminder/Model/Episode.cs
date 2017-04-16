using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Model
{
    class Episode
    {
        public string _overview { get; set; }

        public string _episodeName { get; set; }

        public int _seasonNumber { get; set; }

        public int _episodeNumber { get; set; }

        public int _id { get; set; }

        public int _SeasonId { get; set; }

        public bool _watched { get; set; }

        public DateTime _aired { get; set; }

        public DateTime _lastUpdate { get; set; }

    }
}
