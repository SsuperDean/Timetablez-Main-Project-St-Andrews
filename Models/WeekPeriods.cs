using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class WeekPeriods
    {
        [Key] 
        public int PeriodID { get; set; }
        public string? DailyPeriod { get; set; }
        public int WeekNo { get; set; }
        public string? DayOfWeek { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd{ get; set; }
        public string? PeriodName{ get; set; }
        public bool Accessible { get; set; }
    }

}












