using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TV_Reminder.ViewModel;

namespace TV_Reminder.View
{
    /// <summary>
    /// Interaction logic for SeriesDescription.xaml
    /// </summary>
    public partial class SeriesDescription : UserControl
    {
        public SeriesDescription(int seriesId)
        {
            InitializeComponent();
            ((SeriesDescriptionViewModel)this.DataContext).seriesId = seriesId;
        }

        private void ListBox_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {

        }
    }
}
