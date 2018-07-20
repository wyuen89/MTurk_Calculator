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
        /// <summary>
        /// Default constructor.
        /// </summary>
        public EditWindow()
        {
            InitializeComponent();
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

        /// <summary>
        /// Takes focus off calendar object immediately after click event. This saves the user from having to do an extra click to interact with another element.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Calendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            //Prevents an extra click to take focus off calendar
            UIElement originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }
    }
}
