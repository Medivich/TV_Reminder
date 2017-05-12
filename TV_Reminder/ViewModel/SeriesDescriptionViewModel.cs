using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TV_Reminder.Commands;
using TV_Reminder.Model;
using TV_Reminder.Control;
using System.Collections.ObjectModel;
using System.ComponentModel;
using TV_Reminder.Commands.SeriesDescription;

namespace TV_Reminder.ViewModel
{
    class SeriesDescriptionViewModel : MotherViewModel
    {
        private Series _selectedSeries { get; set; }

        private Episode _selectedEpisode { get; set; }

        //Obejrzany

        ObservableCollection<Season> _seasonList = new ObservableCollection<Season>();

        private int _seriesId;

        public string WatchedButton
        {
            get
            {
                if (_selectedEpisode != null && _selectedEpisode._watched)
                    return "Nieobejrzany";
                else
                    return "Obejrzany";
            }
        }

        public string NextEpisode
        {
            get
            {
                ReadFromDataBase RD = new ReadFromDataBase();
                _selectedEpisode = RD.GetLastEpisode(seriesId);
                if (_selectedEpisode == null)
                    return "Wszystkie dostępne odcinki zostały obejrzane";
                return "Następny: S" + _selectedEpisode.SeasonNumber + "E" + _selectedEpisode.EpisodeNumber + " \"" + _selectedEpisode._episodeName + "\""; 
            }
        }

        public ObservableCollection<Season> SeasonList
        {
            get
            {
                return _seasonList;
            }
            set
            {
                _seasonList = value;
                OnPropertyChanged("SeasonList");
            }
        }

        public Episode SelectedEpisode
        {
            get
            {
                return _selectedEpisode;
            }
            set
            {
                _selectedEpisode = value;
                OnPropertyChanged("SelectedEpisode", "WatchedButton");
            }
        }

        public Series SelectedSeries
        {
            get
            {
                return _selectedSeries;
            }
            set
            {
                _selectedSeries = value;
                OnPropertyChanged("SelectedSeries");
            }
        }

        public void UpdateTree()
        {
            setTreeView();
            OnPropertyChanged("SelectedEpisode", "seriesId");
        }


        public int Rating
        {
            set
            {
                SelectedSeries._rating = value;
                OnPropertyChanged("Star1", "Star2", "Star3", "Star4", "Star5");
            }
        }

        public int seriesId
        {
            get 
            { 
                return _seriesId; 
            }
            set 
            { 
                _seriesId = value;
                setTreeView();
                OnPropertyChanged("seriesId");
            }
        }

        private void setTreeView()
        {
            ObservableCollection<Season> seasonListTemp = new ObservableCollection<Season>();

            ReadFromDataBase RD = new ReadFromDataBase();
            SelectedSeries = RD.GetTvSeries(_seriesId);
            ObservableCollection<Episode> _episodeList = RD.GetAllEpisodes(_seriesId);

            //Wyliczanie ilości sezonów
            int seasonNo = 0;
            foreach(Episode ep in _episodeList)
            {
                if (seasonNo < ep._seasonNumber)
                    seasonNo = ep._seasonNumber;
            }
            //Tworzenie sezonów
            for (int i = 0; i < seasonNo; i++ )
                seasonListTemp.Add(new Season(i + 1));

            //Dodawanie epizodów do poszczególnych sezonów 
            foreach (Episode ep in _episodeList)
            {
                ep.PropertyChanged += item_PropertyChanged;
                seasonListTemp[ep._seasonNumber - 1]._episodeList.Add(ep);
            }

            //Sortowanie
            foreach (Season s in seasonListTemp)
               s.EpisodeList = new ObservableCollection<Episode>(s.EpisodeList.OrderBy(x => x._episodeNumber).ToList());

            SeasonList = seasonListTemp;
            OnPropertyChanged("NextEpisode");
        }

        private void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Episode item = sender as Episode;
            if (item != null && item.IsSelected)
            {
                SelectedEpisode = item;
            }
        }

        private string CheckRating(int stars)
        {
            if (_selectedSeries._rating < stars)
                return @"/Other/Image/starEmpty.png";
            else
                return @"/Other/Image/starFull.png";
        }

        public string Star1 { get { return CheckRating(1); } }
        public string Star2 { get { return CheckRating(2); } }
        public string Star3 { get { return CheckRating(3); } }
        public string Star4 { get { return CheckRating(4); } }
        public string Star5 { get { return CheckRating(5); } }


        private ICommand EpisodeWatchedSeriesDescriptionCommand;

        public ICommand EpisodeWatchedSeriesDescriptionButton
        {
            get
            {
                if (EpisodeWatchedSeriesDescriptionCommand == null)
                    EpisodeWatchedSeriesDescriptionCommand = new EpisodeWatchedSeriesDescription(this);
                return EpisodeWatchedSeriesDescriptionCommand;
            }
        }  

        private ICommand ChangeRatingCommand;

        public ICommand ChangeRatingButton
        {
            get
            {
                if (ChangeRatingCommand == null)
                    ChangeRatingCommand = new ChangeRating(this);
                return ChangeRatingCommand;
            }
        }

        private ICommand AllAboveWatchedCommand;

        public ICommand AllAboveWatchedButton
        {
            get
            {
                if (AllAboveWatchedCommand == null)
                    AllAboveWatchedCommand = new AllAboveWatched(this);
                return AllAboveWatchedCommand;
            }
        }  
        //Wewnętrzna klasa służąca do tworzenia drzewa seriali
        public class Season
        {
            public ObservableCollection<Episode> _episodeList { get; set; }
            public int _number { get; set; }

            public ObservableCollection<Episode> EpisodeList
            {
                get
                {
                    return _episodeList;
                }
                set
                {
                    _episodeList = value;
                }
            }

            public Season(int number)
            {
                this._number = number;
                _episodeList = new ObservableCollection<Episode>();
            }
        }
    }
}
