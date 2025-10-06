using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int VacationPackage { get; set; }
        public int? SuperiorId { get; set; }
        public virtual Employee? Superior { get; set; }
    }
}
