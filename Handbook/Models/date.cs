using System;

namespace Handbook.Models{
    struct Date{
        // A helperclass to format string to the proper date format
        // sql uses 1975-01-01 fomatting
        private string date;
        public Date(){
            date = "1975-01-01";
        }
        public void Today(){
            DateTime thisDay = DateTime.Today;
            int year = thisDay.Year;
            int month = thisDay.Month;
            int day = thisDay.Day;
            date = ""+year+"-"+month+"-"+day;
        }
        public Date ToDate(string str){
            date = str;
            return this;
        }
        public override string ToString()
        {
            return date;
        }
    }
}