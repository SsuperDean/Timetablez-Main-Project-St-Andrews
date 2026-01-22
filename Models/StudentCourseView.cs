using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class StudentCourseView
    {
        [Key]
        public int StudentCourseID { get; set; }
        public string? StudentName { get; set; }
        public string? CourseName { get; set; }
    }

}












