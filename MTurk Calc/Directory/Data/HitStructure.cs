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
        /// <summary>
        /// Gets all HITs in database as an ArrayList of HitInfo
        /// </summary>
        /// <returns>An ArrayList of HitInfos</returns>
        public static ArrayList getHITs()
        {
            return DbUtility.getHITs();
        }
    }
}
