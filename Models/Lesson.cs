using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timetablez.Models
{
    public class Lesson
    {
        public int CourseID { get; set; }
        public int TeacherID { get; set; }
        public int RoomID { get; set; }
        public int PeriodID { get; set; }
        public int GroupID { get; set; }
        public int ColNo { get; set; }
        public int RowNo { get; set; }
        public string GroupName { get; set; }
        public string CourseName { get; set; }
        public string DayOfTheWeek { get; set; }
        public string PeriodName { get; set; }
        public string RoomName { get; set; }
        public Dictionary<int, string> StudentDetail { get; set; }

    }
}
