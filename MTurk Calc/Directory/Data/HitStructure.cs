using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTurk_Calc
{
    public static class HitStructure
    {
        public static ArrayList getHITs()
        {
            return DbUtility.getHITs();
        }
    }
}
