using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using TV_Reminder.Commands;

namespace TV_Reminder.ViewModel
{
    class MainViewModel : MotherViewModel
    {
        UserControl contentWindow = new View.Hello();

        public UserControl content
        {
            get { return contentWindow; }
            set 
            {
                contentWindow = value;
                OnPropertyChanged("content");
            }
        }

        #region Obsluga przyciskow
        private ICommand GoToAddSeries;

        public ICommand AddSeries
        {
            get
            {
                if (GoToAddSeries == null)
                    GoToAddSeries = new GoToAddSeries(this);
                return GoToAddSeries;
            }
        }

        private ICommand GoToUnwatched;

        public ICommand Unwatched
        {
            get
            {
                if (GoToUnwatched == null)
                    GoToUnwatched = new GoToUnwatched(this);
                return GoToUnwatched;
            }
        }

        private ICommand GoToTracked;

        public ICommand Tracked
        {
            get
            {
                if (GoToTracked == null)
                    GoToTracked = new GoToTracked(this);
                return GoToTracked;
            }
        }
        #endregion
    }
    
}
