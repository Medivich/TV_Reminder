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
        ObservableCollection<Series> _SeriesList = new ObservableCollection<Series>();
        ObservableCollection<Poster> _PosterList = new ObservableCollection<Poster>();
        private AddSeriesViewModel main;

        string _search;
        Series _selectedSeries = null;
        Visibility _searchList = Visibility.Hidden, _searching = Visibility.Hidden, _picked = Visibility.Hidden;
        int _found = 0;
        bool _abort = false;

        public ObservableCollection<Poster> PosterList
        {
            set
            {
                _PosterList = value;
                OnPropertyChanged("PosterList");
            }
            get
            {
                return _PosterList;
            }
        }

        public bool Abort
        {
            set
            {
                _abort = value;
                OnPropertyChanged("Abort");
            }
            get
            {
                return _abort;
            }
        }

        public int Found
        {
            set
            {
                _found = value;
                OnPropertyChanged("Found");
            }
            get
            {
                return _found;
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
                    _searchList = Visibility.Hidden;
                    _search = _selectedSeries._title;

                    main = this;
                    Thread t = new Thread(searchForPost);
                    t.IsBackground = true;
                    t.Start();

                    Thread t2 = new Thread(searchForEpisodes);
                    t2.IsBackground = true;
                    t2.Start();

                    Picked = Visibility.Visible;

                    OnPropertyChanged("SelectedSeries", "SearchList", "Search", "PosterList");
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

        private void searchForEpisodes()
        {
            SearchTvdb S = new SearchTvdb();
            S.getMoreSeriesInfo(_selectedSeries._id, this);
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



        public Visibility SearchList
        {
            set
            {
                _searchList = value;
                OnPropertyChanged("SearchList");
            }
            get
            {
                return _searchList;
            }
        }

        public Visibility Searching
        {
            set
            {
                _searching = value;
                OnPropertyChanged("Searching");
            }
            get
            {
                return _searching;
            }
        }

        public Visibility Picked
        {
            set
            {
                _picked = value;
                OnPropertyChanged("Picked");
            }
            get
            {
                return _picked;
            }
        }

        public string Search 
        { 
            set
            {
                _search = value;
            }
            get
            {
                return _search;
            }
        }

        public ObservableCollection<Series> Series 
        { 
            get
            {
                return _SeriesList;
            }
            set
            {           
                Searching = Visibility.Hidden;
                if (value != null)
                {
                    SearchList = Visibility.Visible;
                    _SeriesList = value;
                    OnPropertyChanged("Search", "Series");
                }
            }
        }

        private ICommand SearchSeries;

        public ICommand SearchButton
        {
            get
            {
                if (SearchSeries == null)
                    SearchSeries = new SearchSeries(this);
                return SearchSeries;
            }
        }

        private ICommand AbortSearch;

        public ICommand AbortSearchButton
        {
            get
            {
                if (AbortSearch == null)
                    AbortSearch = new AbortSearch(this);
                return AbortSearch;
            }
        }

        private ICommand AddToDatabase;

        public ICommand AddToDatabaseButton
        {
            get
            {
                if (AddToDatabase == null)
                    AddToDatabase = new AddSeriesToDatabase(this);
                return AddToDatabase;
            }
        }   
    }
}
