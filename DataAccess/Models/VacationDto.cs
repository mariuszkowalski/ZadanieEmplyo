using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class VacationDto
    {
        public int Id { get; set; }
        public DateTime DateSince { get; set; }
        public DateTime DateUntil { get; set; }
        public int NumberOfHours { get; set; }
        public int IsPartialVacation { get; set; }
        public int EmployeeId { get; set; }
    }
}
