using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTurk_Calc
{
    interface IClosableDialog
    {
        /// <summary>
        /// Closes the window
        /// </summary>
        /// <param name="success">Whether or not the intended activity executed successfully or not.</param>
        void Close(bool success);
    }
}
