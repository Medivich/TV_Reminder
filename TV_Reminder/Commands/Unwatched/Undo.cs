using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TV_Reminder.Control;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands.Unwatched
{
    class Undo : MotherCommand
    {
        private readonly UnwatchedViewModel main;
        public Undo(UnwatchedViewModel main)
        {
            if (main == null) throw new ArgumentNullException("TrackedViewModel command");
            this.main = main;
        }

        public override bool CanExecute(object parameter)
        {
            return main._undo;
        }

        override public void Execute(object parameter)
        {
            ReadFromDataBase RD = new ReadFromDataBase();
            UpdateDataBase UD = new UpdateDataBase();
            UD.SetWatched(main._undoWrapper._episode._id, false);

            main.WrapperList.Remove(main._lastAdded);
            main.WrapperList.Add(main._undoWrapper);

            main._undo = false;
            main.reloadWrapperList();
        }
    }
}
