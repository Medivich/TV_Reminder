using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Control;
using TV_Reminder.Model;

namespace TV_Reminder.ViewModel
{
    class TrackedViewModel : MotherViewModel
    {
        ObservableCollection<Series> _seriesList { get; set; }

        public TrackedViewModel()
        {
            Debug.WriteLine("Tracked");
            ReadFromDataBase rd = new ReadFromDataBase();

            seriesList = rd.getAllTvSeries();
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
    }
}
