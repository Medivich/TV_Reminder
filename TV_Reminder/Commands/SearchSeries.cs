using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Input;
using TV_Reminder.ViewModel;
using TV_Reminder.Model;
using System.Drawing;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TV_Reminder.Commands
{
    class SearchSeries : ICommand
    {
        private readonly AddSeriesViewModel main;

        public SearchSeries(AddSeriesViewModel main)
        {
            if (main == null) throw new ArgumentNullException("SearchSeries command");
            this.main = main;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        //Czy kontrolka jest aktywna
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            BitmapImage a = new BitmapImage(new Uri(@"C:\Users\user\Source\Repos\TV_Reminder\TV_Reminder\Other\Image\loading.png"));
            var bitmap = new TransformedBitmap(a,
                        new ScaleTransform(
                            200 / a.Width ,
                            300 / a.Height ));

            ImageSource _image = bitmap;
            

            main.Series.Add(new Series("tytul1", "bla", 3, 4, 1, _image, false));
            Debug.WriteLine("Search");
        }
    }
}
