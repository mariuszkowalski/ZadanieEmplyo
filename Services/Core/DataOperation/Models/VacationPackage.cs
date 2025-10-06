using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Core.DataOperation.Models
{
    public class VacationPackage
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GrantedDays { get; set; }
        public int Year { get; set; }
    }
}
