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
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Windows.Controls.Primitives;

namespace MTurk_Calc
{
    /// <summary>
    /// Interaction logic for AddWindow.xaml
    /// </summary>
    public partial class AddWindow : Window, IClosableDialog
    {
        public SQLiteConnection conn{ get; set; }

        public AddWindow()
        {
            InitializeComponent();

            this.DataContext = new AddViewModel();
        }

        private void Calendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //Prevents an extra click to take focus off calendar
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        public void Close(bool success)
        {
            if (success)
            {
                DialogResult = true;
                Close();
            }

            else
            {
                DialogResult = false;
                Close();
            }
        }
    }
}
