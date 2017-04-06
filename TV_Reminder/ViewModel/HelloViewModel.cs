using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace TV_Reminder.ViewModel
{
    class HelloViewModel
    {
        UserControl contentWindow = new View.Hello();

        public UserControl content
        {
            get { return contentWindow; }
        }
    }
}
