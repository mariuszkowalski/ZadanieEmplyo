using Microsoft.Extensions.Logging;
using Services.Core.DataOperation.Mappers;
using Services.Core.DataOperation.Models;
using Services.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.VacationCalculation
{
    public class VacationCalculationService : IVacationCalculationService
    {
        private readonly ILogger<VacationCalculationService> _log;
        public VacationCalculationService(ILogger<VacationCalculationService> log)
        {
            _log = log;
        }

        public int CountFreeDaysForEmployee(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            try
            {
                employee.CheckIfNotNull(nameof(employee), _log);
                vacationPackage.CheckIfNotNull(nameof(vacationPackage), _log);
                vacations.CheckIfNotNull(nameof(vacations), _log);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Validation failed.");

                throw;
            }

            var year = DateTime.Now.Year;

            var vacationsThisYear = vacations.Where(v => (v.DateSince.Year == year || v.DateUntil.Year == year) && v.EmployeeId == employee.Id).ToList();

            var totalHoursUsed = 0;

            // I assume that the day of vacation corresponds to 8h.
            // I don't have enough information so I assume worst case scenario that sometimes
            // the NumberOfHours is 0 due to errors, 0 != null so it will pass initial check.
            foreach (var vacation in vacationsThisYear)
            {
                if (vacation.NumberOfHours > 0)
                {
                    totalHoursUsed += vacation.NumberOfHours;
                }
                else
                {
                    var days = (vacation.DateUntil.Date - vacation.DateSince.Date).Days + 1;
                    totalHoursUsed += days * 8;
                }
            }

            var usedDays = totalHoursUsed / 8;
            var remainingDays = vacationPackage.GrantedDays - usedDays;

            return remainingDays;
        }

        public bool IfEmployeeCanRequestVacation(Employee employee, List<Vacation> vacations, VacationPackage vacationPackage)
        {
            var remainingDays = CountFreeDaysForEmployee(employee, vacations, vacationPackage);
            
            var canRrequestVacation = remainingDays > 0;

            return canRrequestVacation;
        }

    }
}
