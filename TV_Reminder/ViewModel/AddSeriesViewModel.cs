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
    class AddSeriesViewModel : SeriesViewModel
    {
        ObservableCollection<Series> _seriesList = new ObservableCollection<Series>();
        ObservableCollection<Poster> _posterList = new ObservableCollection<Poster>();
        private AddSeriesViewModel main;

        string _searchQuery;
        Series _selectedSeries = null;
        Visibility _replyList = Visibility.Hidden, _searchingScreen = Visibility.Hidden, _seriesInfo = Visibility.Hidden;
        int _foundSeries = 0;
        bool _abortSearch = false;

        public ObservableCollection<Poster> PosterList
        {
            set
            {
                _posterList = value;
                OnPropertyChanged("PosterList");
            }
            get
            {
                return _posterList;
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
                    _selectedSeries._episode_number = value;
                    OnPropertyChanged("EpisodeNumber");
                }
            }
            get
            {
                if (_selectedSeries != null)
                    return _selectedSeries._episode_number;
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
                    _selectedSeries = value;
                    ReplyList = Visibility.Hidden;
                    _searchQuery = _selectedSeries._title;

                    main = this;
                    Thread posterSearcher = new Thread(searchForPost);
                    posterSearcher.IsBackground = true;
                    posterSearcher.Start();

                    Thread episodeNumberSearcher = new Thread(searchForEpisodeNumber);
                    episodeNumberSearcher.IsBackground = true;
                    episodeNumberSearcher.Start();

                    SeriesInfo = Visibility.Visible;

                    OnPropertyChanged("SelectedSeries", "SearchQuery", "PosterList");
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
                    _selectedSeries._memoryStream = value._memoryStream;
            }
            get
            {
                if (_selectedSeries != null)
                    return new Poster(_selectedSeries._memoryStream);
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
