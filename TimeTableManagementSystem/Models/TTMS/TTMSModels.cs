using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeTableManagementSystem.Models.TTMS
{
    public class TTMSModels
    {
        public string SubjectName { get; set; }
        public int WorkingDayPerWeek { get; set; }       
        public int SubjectPerday { get; set; }
        public int TotalSubjects { get; set; }
        public int TotalHrsPerWeek { get; set; }

        public string AllSubjectsName { get; set; }
        public string AllSubjectsHour { get; set; }

    }
}