using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class WeeklySchedule
    {
        [Key]
        public int WeeklyScheduleID { get; set; }
        public int GroupID { get; set; }
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public int RoomID { get; set; }
        public int PeriodID { get; set; }
    }

}












