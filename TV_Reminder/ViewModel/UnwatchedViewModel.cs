using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TV_Reminder.Commands.Unwatched;
using TV_Reminder.Control;
using TV_Reminder.Model;

namespace TV_Reminder.ViewModel
{
    class UnwatchedViewModel : MotherViewModel
    {
        ObservableCollection<Wrapper> _wrapperList = new ObservableCollection<Wrapper>();
        bool _showAll = false, _sortByRating = true, _showBanners = true;

        public Wrapper _selectedWrapper;

        public UnwatchedViewModel()
        {
            WrapperList = getLastUnwatched();
            WrapperList = SortListByRate(WrapperList);
        }

        public bool ShowAll
        {
            set
            {
                this._showAll = value;
                WrapperList = getAllUnwatched();
                ShowBanners = false;
                OnPropertyChanged("ShowAll", "ShowOne");
            }
            get
            {
                return this._showAll;
            }
        }


        private ObservableCollection<Wrapper> getLastUnwatched()
        {
            ObservableCollection<Wrapper> _List = new ObservableCollection<Wrapper>();
            ReadFromDataBase RD = new ReadFromDataBase();
            ObservableCollection<Series> _seriesList = RD.getAllTvSeries();

            foreach (Series s in _seriesList)
            {
                Episode ep = RD.GetLastEpisode(s._id);
                if (ep != null)
                    _List.Add(new Wrapper(ep, s._banner, s._seriesName, _showBanners, s._rating));
            }

            return _List;
        }

        private ObservableCollection<Wrapper> getAllUnwatched()
        {
            ObservableCollection<Wrapper> _List = new ObservableCollection<Wrapper>();

            ReadFromDataBase RD = new ReadFromDataBase();
            ObservableCollection<Series> _seriesList = RD.getAllTvSeries();

            foreach (Series s in _seriesList)
            {
                ObservableCollection<Episode> _episodeList = RD.GetAllUnWatchedEpisodes(s._id);
                foreach(Episode e in _episodeList)
                {
                    _List.Add(new Wrapper(e, s._banner, s._seriesName, _showBanners, s._rating));
                }          
            }

            return _List;
        }

        private ObservableCollection<Wrapper> SortListByRate(ObservableCollection<Wrapper> _in)
        {
            return new ObservableCollection<Wrapper>(_in.OrderByDescending(x => x._rating).ThenBy(y => y._seriesName).ToList());
        }

        private ObservableCollection<Wrapper> SortListByName(ObservableCollection<Wrapper> _in)
        {
            return new ObservableCollection<Wrapper>(_in.OrderBy(y => y._seriesName).ToList());
        }

        public bool ShowOne
        {
            set
            {
                this._showAll = !value;
                WrapperList = getLastUnwatched();
                OnPropertyChanged("ShowAll", "ShowOne");
            }
            get
            {
                return !this._showAll;
            }
        }

        public bool SortByRating
        {
            set
            {
                this._sortByRating = value;
                WrapperList = SortListByRate(WrapperList);
                OnPropertyChanged("SortByRating", "SortAlfabetically");
            }
            get
            {
                return this._sortByRating;
            }
        }

        public bool ShowBanners
        {
            set
            {
                foreach(Wrapper w in _wrapperList)
                {
                    w.ShowBanner = value;
                }
                _showBanners = value;
                OnPropertyChanged("ShowBanners", "Banner", "SeriesName");
            }
            get
            {
                return this._showBanners;
            }
        }

        public bool SortAlfabetically
        {
            set
            {
                this._sortByRating = !value;
                WrapperList = SortListByName(WrapperList);
                OnPropertyChanged("SortByRating", "SortAlfabetically");
            }
            get
            {
                return !this._sortByRating;
            }
        }


        public Wrapper SelectedWrapper
        {
            set
            {
                if (value != null)
                {
                    this._selectedWrapper = value;
                    Debug.WriteLine(value._episode._episodeName);
                    OnPropertyChanged("SelectedWrapper");
                }
            }
            get
            {
                return this._selectedWrapper;
            }
        }

        public ObservableCollection<Wrapper> WrapperList
        {
            set
            {
                if (value != null)
                {
                    this._wrapperList = value;
                    OnPropertyChanged("WrapperList");
                }
            }
            get
            {
                return this._wrapperList;
            }
        }

        private ICommand UndoCommand;

        public ICommand UndoButton
        {
            get
            {
                if (UndoCommand == null)
                    UndoCommand = new Undo(this);
                return UndoCommand;
            }
        }

        private ICommand WatchedCommand;

        public ICommand WatchedButton
        {
            get
            {
                if (WatchedCommand == null)
                    WatchedCommand = new Watched(this);
                return WatchedCommand;
            }
        }

        private ICommand LookForCommand;

        public ICommand LookForButton
        {
            get
            {
                if (LookForCommand == null)
                    LookForCommand = new LookFor(this);
                return LookForCommand;
            }
        }
    }
}
