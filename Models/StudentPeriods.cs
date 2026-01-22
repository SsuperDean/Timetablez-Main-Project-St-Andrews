using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
//using System.Threading.Tasks;


namespace Timetablez.Models
{
    public class StudentPeriods
    {
        [Key]
        public int StudentPeriodID { get; set; }
        public int StudentID { get; set; }
        public int PeriodID { get; set; }
        public bool Accessible { get; set; }
    }

}












