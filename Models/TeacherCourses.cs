using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class TeacherCourses
    {
        [Key]
        public int TeacherCourseID { get; set; }
        public int TeacherID { get; set; }
        public int CourseID { get; set; }
        public bool Accessible { get; set; }
    }

}












