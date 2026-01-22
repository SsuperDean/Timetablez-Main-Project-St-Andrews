using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class StudentGroup
    {
        [Key]
        public int StudentGroupID { get; set; }
        public int StudentID { get; set; }
        public int GroupID { get; set; }
    }

}












