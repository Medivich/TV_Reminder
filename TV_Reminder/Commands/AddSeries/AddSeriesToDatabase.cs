using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TV_Reminder.Control;
using TV_Reminder.Model;
using TV_Reminder.ViewModel;

namespace TV_Reminder.Commands
{
    class AddSeriesToDatabase : MotherCommand
    {
        private readonly AddSeriesViewModel main;
        private Object thisLock = new Object();
        private List<Episode> ep = new List<Episode>();

        public AddSeriesToDatabase(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("AddToDatabase command");
            this.main = main;
        }

        //Czy kontrolka jest aktywna
        override public bool CanExecute(object parameter)
        {
            if (main.SeriesInfo == System.Windows.Visibility.Visible && !main.getExist())
                return true;
            else
                return false;
        }

        override public void Execute(object parameter)
        {
            main.LoadingScreen = Visibility.Visible;

            Thread t0 = new Thread(add);
            t0.IsBackground = true;
            t0.Start();   
        }

        //Tworzy wątki, które pobierają odcinki (w jednej paczce przechodzi max. 100 odcinków)
        private void add()
        {
            DownloadEpisodes ED = new DownloadEpisodes();
            ep = ED.getEpisodes(main.SelectedSeries._id);
     
            try
            {
                AddToDataBase add = new AddToDataBase();
                add.addTvSeries(main.SelectedSeries);

                foreach(Episode e in ep)
                    add.addEpisode(main.SelectedSeries._id, e);
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            Application.Current.Dispatcher.Invoke(new Action(() => main.setExist(true)));
            Application.Current.Dispatcher.Invoke(new Action(() => main.LoadingScreen = Visibility.Hidden));
        }
    }
}
