using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class TeacherPeriods
    {
        [Key]
        public int TeacherPeriodID { get; set; }
        public int TeacherID { get; set; }
        public int PeriodID { get; set; }
        public bool Accessible { get; set; }
    }

}












