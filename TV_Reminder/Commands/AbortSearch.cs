using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.Control;

namespace TV_Reminder.Commands
{
    class AbortSearch : MotherCommand
    {
        private readonly AddSeriesViewModel main;

        public AbortSearch(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("AbortSearch command");
            this.main = main;
        }

        override public void Execute(object parameter)
        {
            main.AbortSearch = true;
        }
    }
}
