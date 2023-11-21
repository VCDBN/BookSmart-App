using System;
using System.Collections.Generic;

namespace BookSmart
{
    //IComparer class that uses custom comparison logic for sorting the list in SortCallNumbers().
    class CallNumberComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            var partsX = x.Split(' ');
            var partsY = y.Split(' ');
            //Numeric parts of the call numbers are taken
            double numericPartX = double.Parse(partsX[0]);
            double numericPartY = double.Parse(partsY[0]);

            //Numeric parts are compared
            int numericComparison = numericPartX.CompareTo(numericPartY);

            //If the numbers are not equal, the numeric comparison is returned
            if (numericComparison != 0)
            {
                return numericComparison;
            }
            //If the numbers are equal, then the string (surname) part is compared, and string comparison returned
            else
            {
                return string.Compare(partsX[1], partsY[1], StringComparison.Ordinal);
            }
            //This ensures that the list is ordered numerically and then alphabetically where any numbers are identical
        }
    }
}
