using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTurk_Calc
{
    public enum Status
    {
        /// <summary>
        /// Waiting to be approved or rejected.
        /// </summary>
        Pending,

        /// <summary>
        /// Approved but not paid yet.
        /// </summary>
        Approved,

        /// <summary>
        /// Approved and paid.
        /// </summary>
        Paid
    }
}
