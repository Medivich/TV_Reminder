using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.Commands;

namespace TV_Reminder.ViewModel
{
    class AddSeriesViewModel : MotherViewModel
    {




        private ICommand SearchSeries;

        public ICommand Search
        {
            get
            {
                if (SearchSeries == null)
                    SearchSeries = new SearchSeries(this);
                return SearchSeries;
            }
        }
    }
}
