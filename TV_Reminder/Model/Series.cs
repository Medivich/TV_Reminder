using Newtonsoft.Json;
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
        [JsonProperty("overview")]
        public string _overview { get; set; }

        [JsonProperty("seriesName")]
        public string _seriesName { get; set; }

        [JsonProperty("airedEpisodes")]
        public int _airedEpisodes { get; set; }

        [JsonProperty("id")]
        public int _id { get; set; }

        public int _rating { get; set; }

        public bool _update { get; set; }

        public byte[] _poster { get; set; }

        public Series()
        {
            ;
        }

        public Series(string seriesName, string overview, int id, byte[] poster)
        {
            this._id = id;
            this._seriesName = seriesName;
            this._poster = poster;
            this._overview = overview;
        }

        [JsonConstructor]
        public Series(string seriesName, string overview, int id)
        {
            this._id = id;
            this._seriesName = seriesName;
            this._overview = overview;
        }
    }
}
