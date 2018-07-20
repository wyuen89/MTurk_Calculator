using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Collections.ObjectModel;

namespace MTurk_Calc
{
    class HitExpand: Expander
    {
        /// <summary>
        /// The date for the group of HITs in hitList.
        /// </summary>
        public String header { get; set; }

        /// <summary>
        /// The collection of all the HITs in the database as HitInfo objects.
        /// </summary>
        public ObservableCollection<HitInfo> hitList { get; set; }
    }
}
