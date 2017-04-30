using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Model 
{
    class Wrapper : MotherViewModel
    {
        public int _rating { get; set; }

        public byte[] _banner { get; set; }

        public string _seriesName { get; set; }

        public Episode _episode { get; set; }

        public bool _showBanners;

        public Wrapper(Episode episode, byte[] banner, string seriesName, bool showBanners, int rating)
        {
            this._seriesName = seriesName;
            this._episode = episode;
            this._banner = banner;
            this._showBanners = showBanners;
            this._rating = rating;
        }

        public Wrapper(Episode episode)
        {
            this._episode = episode;
        }

        public bool ShowBanner
        {
            set
            {
                _showBanners = value;
                OnPropertyChanged("Banner", "ShowBanner", "SeriesNameVisibility");
            }
            get
            {
                return this._showBanners;
            }
        }
        

        public Visibility Banner
        {
            get
            {
                if (_showBanners)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }

        public Visibility SeriesNameVisibility
        {
            get
            {
                if (!_showBanners)
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;
            }
        }
    }
}
