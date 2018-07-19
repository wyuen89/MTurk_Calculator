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
        public String header { get; set; }

        public ObservableCollection<HitInfo> hitList { get; set; }
    }
}
