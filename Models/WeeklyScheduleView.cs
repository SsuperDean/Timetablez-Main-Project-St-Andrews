using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class WeeklyScheduleView
    {
        [Key]
        public int WeeklyScheduleID { get; set; }
        public string? StudentName { get; set; }
        public string? DailyPeriod { get; set; }
        public string? GroupName { get; set; }
        public string? CourseName{ get; set; }
        public string? RoomCode { get; set; }
        public string? RoomDescription { get; set; }
        public string? TeacherName { get; set; }
        public string? PeriodName { get; set; }
        public String? TimeStart { get; set; }
        public String? TimeEnd { get; set; }
        public string? DayOfTheWeek { get; set; }
    }

}












