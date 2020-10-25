using System;
using System.Collections.Generic;
using System.Text;

namespace ADPC.Ship
{
    public class UnloadFilter
    {
        //Filtering by Time ( Year, Year-Month, Year-Month-Day )
        //{year}*, {year}{month}*, {year}{month}{day}*
        public string Year, Month, Day;
        /// <summary>
        /// UnloadFilter for LocalShip path pattern.
        /// </summary>
        /// <param name="year">Four numbers ex)2020</param>
        /// <param name="month">Two numbers ex)05</param>
        /// <param name="day">Two numbers ex)14</param>
        public UnloadFilter(string year, string month, string day)
        {
            Year = year;
            Month = month;
            Day = day;
        }
        public UnloadFilter(string year, string month) : this(year, month, "") { }
        public UnloadFilter(string year) : this(year, "", "") { }
        /// <summary>
        /// ToString()
        /// 2020*
        /// 202010*
        /// 20201011*
        /// </summary>
        /// <returns>Only number date and last char must be '*' </returns>
        public override string ToString()
        {
            return $"{Year}{Month}{Day}*";
        }
    }
}
