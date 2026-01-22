using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class Courses
    {
        [Key] 
        public int CourseID { get; set; }
        public string? Code { get; set; }
        public string? Name{ get; set; }
        public int ExamBoardID { get; set; }
        public bool Active{ get; set; }
        public int RoomTypeID { get; set; }
        public int GroupSize { get; set; }
        public int MaxStudents { get; set; }
        public int SessionsPerWeek { get; set; }
    }

}












