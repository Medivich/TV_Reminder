using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TV_Reminder.Commands;
using TV_Reminder.Control;
using TV_Reminder.Model;

namespace TV_Reminder.ViewModel
{
    class TrackedViewModel : MotherViewModel
    {
        ObservableCollection<Series> _seriesList { get; set; }
        Series _selectedSeries { get; set; }
        private Visibility _loadingScreen = Visibility.Hidden, _seriesListVisibility = Visibility.Visible;
        private UserControl _description = null;

        public TrackedViewModel()
        {
            LoadingScreen = Visibility.Visible;
            Thread t = new Thread(new ParameterizedThreadStart(loadSeries));
            t.IsBackground = true;
            t.Start(this); 
        }

        public ObservableCollection<Series> seriesList
        {
            get
            {
                return this._seriesList;
            }
            set
            {
                this._seriesList = value;
                OnPropertyChanged("seriesList");
            }
        }

        public Series selectedSeries
        {
            get
            {
                return this._selectedSeries;
            }
            set
            {
                this._selectedSeries = value;
                OnPropertyChanged("selectedSeries");
            }
        }

        public UserControl Description
        {
            set
            {
                _description = value;
                OnPropertyChanged("Description");
            }
            get
            {
                return _description;
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

        public Visibility SeriesListVisibility
        {
            set
            {
                _seriesListVisibility = value;
                OnPropertyChanged("SeriesListVisibility");
            }
            get
            {
                return _seriesListVisibility;
            }
        }




        private void loadSeries(object main)
        {
            try
            {
                TrackedViewModel vm = (TrackedViewModel)main;
                ReadFromDataBase RD = new ReadFromDataBase();
                ObservableCollection<Series> _seriesList = new ObservableCollection<Series>();
                _seriesList = RD.getAllTvSeries();
                _seriesList = new ObservableCollection<Series>(_seriesList.OrderBy(x => x._seriesName).ToList());

                Application.Current.Dispatcher.Invoke(new Action(() => vm.seriesList = _seriesList));
            }
            catch (Exception)
            {
                ;
            }
            finally
            {
                TrackedViewModel vm = (TrackedViewModel)main;
                Application.Current.Dispatcher.Invoke(new Action(() => vm.LoadingScreen = Visibility.Hidden));
            }
        }

        private ICommand DeleteSelectedSeriesCommand;

        public ICommand DeleteSelectedSeriesButton
        {
            get
            {
                if (DeleteSelectedSeriesCommand == null)
                    DeleteSelectedSeriesCommand = new DeleteSelectedSeries(this);
                return DeleteSelectedSeriesCommand;
            }
        }

        private ICommand ShowSelectedSeriesCommand;

        public ICommand ShowSelectedSeriesButton
        {
            get
            {
                if (ShowSelectedSeriesCommand == null)
                    ShowSelectedSeriesCommand = new ShowSelectedSeries(this);
                return ShowSelectedSeriesCommand;
            }
        }

        private ICommand UpdateSelectedSeriesCommand;

        public ICommand UpdateSelectedSeriesButton
        {
            get
            {
                if (UpdateSelectedSeriesCommand == null)
                    UpdateSelectedSeriesCommand = new UpdateSelectedSeries(this);
                return UpdateSelectedSeriesCommand;
            }
        }

        private ICommand UpdateManySeriesCommand;

        public ICommand UpdateManySeriesButton
        {
            get
            {
                if (UpdateManySeriesCommand == null)
                    UpdateManySeriesCommand = new UpdateManySeries(this);
                return UpdateManySeriesCommand;
            }
        }
    }
}
