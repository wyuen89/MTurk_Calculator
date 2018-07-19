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

        /// <summary>
        /// Constructor
        /// </summary>
        public AddWindow()
        {
            InitializeComponent();

            this.DataContext = new AddViewModel();
        }

        /// <summary>
        /// Takes focus off calendar object immediately after click event. This saves the user from having to do an extra click to interact with another element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Closes window and sets DialogResult based on if the HIT was added successfully.
        /// </summary>
        /// <param name="success">True if HIT was added successfully, false if not.</param>
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
