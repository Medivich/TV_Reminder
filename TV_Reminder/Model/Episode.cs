using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Model
{
    class Episode : MotherViewModel
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

        public Episode()
        {
            ;
        }

        public string EpisodeNumber
        {
            get
            {
                if (_episodeNumber < 10)
                    return "0" + _episodeNumber;
                else
                    return "" + _episodeNumber;
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set { _isSelected = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Aired
        {
            get
            {
                if (_aired.Year > 1950)
                    return string.Format("Data emisji: {0:dd/MM/yyyy}", _aired);
                else
                    return "Data emisji: -";
            }
        }

        public string SeasonNumber
        {
            get
            {
                if (_seasonNumber < 10)
                    return "0" + _seasonNumber;
                else
                    return "" + _seasonNumber;
            }
        }

        public bool Watched
        {
            set 
            { 
                this._watched = value;
                OnPropertyChanged("pic");
            }
        }

        public string pic
        {
            get
            {
                if (DateTime.Today <= _aired || _aired.Year < 1950)
                    return @"/Other/Image/notAvailable.png";
                else if (_watched)
                    return @"/Other/Image/Watched.png";
                else
                    return @"/Other/Image/Unwatched.png";
            }
        }
    }
}
