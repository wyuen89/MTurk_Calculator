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
using System.Windows.Controls.Primitives;

namespace MTurk_Calc
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class EditWindow : Window, IClosableDialog
    {
        public EditWindow()
        {
            InitializeComponent();
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
        private void Calendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //Prevents an extra click to take focus off calendar
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            //Changes displayed text in TextBox "date"
            date.Text = calendar.SelectedDate.Value.Date.ToShortDateString();
        }
    }
}
