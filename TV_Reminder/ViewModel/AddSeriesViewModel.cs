using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TV_Reminder.Commands;
using TV_Reminder.Model;
using TV_Reminder.Control;
using System.Threading;

namespace TV_Reminder.ViewModel
{
    class AddSeriesViewModel : MotherViewModel
    {
        ObservableCollection<Series> _seriesList = new ObservableCollection<Series>();
        ObservableCollection<Poster> _posterList = new ObservableCollection<Poster>();
        ObservableCollection<byte[]> _bannerList = new ObservableCollection<byte[]>();

        private AddSeriesViewModel main;

        string _searchQuery;
        Series _selectedSeries = null;
        Visibility _replyList = Visibility.Hidden, _loadingScreen = Visibility.Hidden,
            _searchingScreen = Visibility.Hidden, _seriesInfo = Visibility.Hidden;
        int _foundSeries = 0;
        bool _abortSearch = false, _seriesExist = false;
        

        public void setExist(bool exist)
        {
            this._seriesExist = exist;
            OnPropertyChanged("SeriesExist");
        }

        public bool getExist()
        {
            return this._seriesExist;
        }

        public string SeriesExist
        {
            get
            {
                if (_seriesExist)
                    return "Serial dodany do bazy";
                else
                    return "Dodaj serial do bazy";
            }

        }   

        public byte[] SelectedBanner
        {
            set
            {
                if (value != null)
                    _selectedSeries._banner = value;
            }
            get
            {
                if (_selectedSeries != null && _selectedSeries._banner != null)
                    return _selectedSeries._banner;
                else
                    return null;
            }
        }
 

        public ObservableCollection<Poster> PosterList
        {
            set
            {
                _posterList = value;
                if (value != null && _posterList.Count > 0)
                {
                    SelectedPoster = _posterList[0];
                    OnPropertyChanged("SelectedPoster");
                }

                OnPropertyChanged("PosterList");
            }
            get
            {
                return _posterList;
            }
        }

        public ObservableCollection<byte[]> BannerList
        {
            set
            {
                _bannerList = value;
                OnPropertyChanged("BannerList");
            }
            get
            {
                return _bannerList;
            }
        }

        public bool AbortSearch
        {
            set
            {
                _abortSearch = value;
                OnPropertyChanged("AbortSearch");
            }
            get
            {
                return _abortSearch;
            }
        }

        public int FoundSeries
        {
            set
            {
                _foundSeries = value;
                OnPropertyChanged("FoundSeries");
            }
            get
            {
                return _foundSeries;
            }
        }

        public int EpisodeNumber
        {
            set
            {
                if (value != 0 && _selectedSeries != null)
                {
                    _selectedSeries._airedEpisodes = value;
                    OnPropertyChanged("EpisodeNumber");
                }
            }
            get
            {
                if (_selectedSeries != null)
                    return _selectedSeries._airedEpisodes;
                else
                    return 0;
            }
        }

        public Series SelectedSeries
        {
            set
            {
                if (value != null)
                {
                    PosterList.Clear();
                    BannerList.Clear();
                    _selectedSeries = value;
                    ReplyList = Visibility.Hidden;
                    _searchQuery = _selectedSeries._seriesName;

                    ReadFromDataBase RD = new ReadFromDataBase();
                    _seriesExist = RD.SeriesExist(_selectedSeries._id);


                    main = this;
                    Thread posterSearcher = new Thread(searchForPost);
                    posterSearcher.IsBackground = true;
                    posterSearcher.Start();

                    Thread bannerSearcher = new Thread(searchForBann);
                    bannerSearcher.IsBackground = true;
                    bannerSearcher.Start();

                    Thread episodeNumberSearcher = new Thread(searchForEpisodeNumber);
                    episodeNumberSearcher.IsBackground = true;
                    episodeNumberSearcher.Start();

                    SeriesInfo = Visibility.Visible;

                    OnPropertyChanged("SelectedSeries", "SearchQuery", "PosterList", "BannerList", "SeriesExist");
                }
            }
            get
            {
                return _selectedSeries;
            }
        }

        private void searchForPost()
        {
            SearchTvdb S = new SearchTvdb();
            S.SearchForAllPosters(_selectedSeries._id, this);
        }

        private void searchForBann()
        {
            SearchTvdb S = new SearchTvdb();
            S.SearchForAllBanners(_selectedSeries._id, this);
        }

        private void searchForEpisodeNumber()
        {
            SearchTvdb S = new SearchTvdb();
            S.getOverallEpisodesNumber(_selectedSeries._id, this);
        }

        public Poster SelectedPoster
        {
            set
            {
                if(value != null)
                    _selectedSeries._poster = value._memoryStream;
            }
            get
            {
                if (_selectedSeries != null && _selectedSeries._poster != null)
                    return new Poster(_selectedSeries._poster);
                else
                    return null;
            }
        }



        public Visibility ReplyList
        {
            set
            {
                _replyList = value;
                OnPropertyChanged("ReplyList");
            }
            get
            {
                return _replyList;
            }
        }

        public Visibility SearchingScreen
        {
            set
            {
                _searchingScreen = value;
                OnPropertyChanged("SearchingScreen");
            }
            get
            {
                return _searchingScreen;
            }
        }

        public Visibility SeriesInfo
        {
            set
            {
                _seriesInfo = value;
                OnPropertyChanged("SeriesInfo");
            }
            get
            {
                return _seriesInfo;
            }
        }

        public Visibility LoadingScreen
        {
            set
            {
                _loadingScreen = value;
                OnPropertyChanged("LoadingScreen");
            }
            get
            {
                return _loadingScreen;
            }
        }

        public string SearchQuery 
        { 
            set
            {
                _searchQuery = value;
            }
            get
            {
                return _searchQuery;
            }
        }

        public ObservableCollection<Series> Series 
        { 
            get
            {
                return _seriesList;
            }
            set
            {
                LoadingScreen = Visibility.Hidden;
                SearchingScreen = Visibility.Hidden;
                if (value != null)
                {
                    ReplyList = Visibility.Visible;
                    _seriesList = value;
                    OnPropertyChanged("SearchQuery", "Series");
                }
            }
        }

        private ICommand SearchSeriesCommand;

        public ICommand SearchButton
        {
            get
            {
                if (SearchSeriesCommand == null)
                    SearchSeriesCommand = new SearchSeries(this);
                return SearchSeriesCommand;
            }
        }

        private ICommand AbortSearchCommand;

        public ICommand AbortSearchButton
        {
            get
            {
                if (AbortSearchCommand == null)
                    AbortSearchCommand = new AbortSearch(this);
                return AbortSearchCommand;
            }
        }

        private ICommand AddToDatabaseCommand;

        public ICommand AddToDatabaseButton
        {
            get
            {
                if (AddToDatabaseCommand == null)
                    AddToDatabaseCommand = new AddSeriesToDatabase(this);
                return AddToDatabaseCommand;
            }
        }   
    }
}
