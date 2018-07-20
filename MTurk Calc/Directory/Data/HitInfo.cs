using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTurk_Calc
{
    class HitInfo
    {
        /// <summary>
        /// The HIT's id
        /// </summary>
        public String id { get; set; }

        /// <summary>
        /// The HIT's date
        /// </summary>
        public String date { get; set; }

        /// <summary>
        /// The HIT's requester name
        /// </summary>
        public String requester { get; set; }

        /// <summary>
        /// The HIT's name
        /// </summary>
        public String name { get; set; }

        /// <summary>
        /// The HIT's base amount paid
        /// </summary>
        public String amt { get; set; }

        /// <summary>
        /// The HIT's bonus amount paid
        /// </summary>
        public String bonus { get; set; }

        /// <summary>
        /// The HIT's status; pending, approved, or paid.
        /// </summary>
        public String status { get; set; }
    }
}
