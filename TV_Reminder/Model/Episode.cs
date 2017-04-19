using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TV_Reminder.Model
{
    class Episode
    {

        [JsonProperty("overview")]
        public string _overview { get; set; }

        [JsonProperty("episodeName")]
        public string _episodeName { get; set; }

        [JsonProperty("airedSeason")]
        public int _seasonNumber { get; set; }

        [JsonProperty("airedEpisodeNumber")]
        public int _episodeNumber { get; set; }

        [JsonProperty("id")]
        public int _id { get; set; }

        public int _absoluteNumber { get; set; }

        public bool _watched { get; set; }

        [JsonProperty("firstAired")]
        public DateTime _aired { get; set; }

        [JsonProperty("lastUpdated")]
        public int _lastUpdate { get; set; }

        [JsonConstructor]
        public Episode(string overview, string episodeName, int airedEpisodeNumber, int id, DateTime firstAired, int lastUpdated)
        {
            this._overview = overview;
            this._episodeName = episodeName;
            this._episodeNumber = airedEpisodeNumber;
            this._id = id;
            this._aired = firstAired;
            this._lastUpdate = lastUpdated;
        }
    }
}
