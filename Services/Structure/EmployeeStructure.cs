using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Structure
{
    public class EmployeeStructure
    {
        public int EmployeeId { get; set; }
        public int SuperiorId { get; set; }
        public int Rank { get; set; }
    }
}
