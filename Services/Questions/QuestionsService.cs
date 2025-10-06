using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Core.DataOperation.Interfaces;
using Services.Core.DataOperation.Models;
using Services.Helpers;
using Services.Structure;
using Services.VacationCalculation;
using DataAccess.Models;
using Spectre.Console;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Questions
{
    public class QuestionsService : IQuestionsService
    {
        private readonly ILogger<QuestionsService> _log;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationPackageService _vacationPackageService;
        private readonly IVacationService _vacationService;
        private readonly IVacationCalculationService _calculationService;
        private readonly EmployeeStructureService _employeeStructureService;

        public QuestionsService(ILogger<QuestionsService> log, IEmployeeService employeeService, IVacationPackageService vacationPackageService, 
            IVacationService vacationService, IVacationCalculationService calculationService, EmployeeStructureService employeeStructureService)
        {
            _log = log;
            _employeeService = employeeService;
            _vacationPackageService = vacationPackageService;
            _vacationService = vacationService;
            _calculationService = calculationService;
            _employeeStructureService = employeeStructureService;
        }

        public void EmployeeStructureExample()
        {
            var employee1 = new Employee { Id = 1, Name = "Jan Kowalski" };
            var employee2 = new Employee { Id = 2, Name = "Kamil Nowak", SuperiorId = 1, Superior = employee1 };
            var employee3 = new Employee { Id = 3, Name = "Anna Mariacka", SuperiorId = 1, Superior = employee1 };
            var employee4 = new Employee { Id = 4, Name = "Andrzej Abracki", SuperiorId = 2, Superior = employee2 };

            var employees = new List<Employee>
            {
                employee1, employee2, employee3, employee4
            };

            _employeeStructureService.FillEmployeesStructure(employees);

            var row1 = _employeeStructureService.GetSuperiorRowOfEmployee(2, 1).ToString();
            var row2 = _employeeStructureService.GetSuperiorRowOfEmployee(4, 3).ToString();
            var row3 = _employeeStructureService.GetSuperiorRowOfEmployee(4, 1).ToString();

            var ans1 = string.IsNullOrEmpty(row1) ? "null" : row1;
            var ans2 = string.IsNullOrEmpty(row2) ? "null" : row2;
            var ans3 = string.IsNullOrEmpty(row3) ? "null" : row3;

            _log.InfoWithConsole("Checking the managment structure.");
            _log.InfoWithConsole($"Result of: _employeeStructureService.GetSuperiorRowOfEmployee(2, 1); = {ans1}");
            _log.InfoWithConsole($"Result of: _employeeStructureService.GetSuperiorRowOfEmployee(4, 3); = {ans2}");
            _log.InfoWithConsole($"Result of: _employeeStructureService.GetSuperiorRowOfEmployee(4, 1); = {ans3}");
        }

        public int CalculateRemainingVacationDaysExample()
        {
            _log.InfoWithConsole($"[green]CalculateRemainingVacationDaysExample:[/]");
            
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };

            var employee = new Employee { Id = 1, Name = "Mariusz Kowalski", VacationPackage = 1 };

            var vacations = new List<Vacation>
            {
                new() {
                    EmployeeId = 1,
                    DateSince = new DateTime(DateTime.Now.Year, 1, 15),
                    DateUntil = new DateTime(DateTime.Now.Year, 1, 16),
                    NumberOfHours = 16
                },
                new() {
                    EmployeeId = 1,
                    DateSince = new DateTime(DateTime.Now.Year, 3, 10),
                    DateUntil = new DateTime(DateTime.Now.Year, 3, 10),
                    NumberOfHours = 8
                }
            };

            
            var vacationPackage = new VacationPackage { Id = 1, Name = "Standard", GrantedDays = 26, Year = 2025 };

            int remainingDays = _calculationService.CountFreeDaysForEmployee(employee, vacations, vacationPackage);

            _log.InfoWithConsole($"[Green]Remaining days: {remainingDays.ToString()}.[/]");


            return remainingDays;
        }

        public bool CheckIfEmployeeCanTakeVacationExample()
        {
            _log.InfoWithConsole($"[green]CheckIfEmployeeCanTakeVacationExample:[/]");

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore
            };
            
            var employee = new Employee { Id = 1, Name = "Mariusz Kowalski", VacationPackage = 1 };

            var vacations = new List<Vacation>
            {
                new Vacation {
                    EmployeeId = 1,
                    DateSince = new DateTime(DateTime.Now.Year, 1, 15),
                    DateUntil = new DateTime(DateTime.Now.Year, 1, 20),
                    NumberOfHours = 40
                },
                new Vacation {
                    EmployeeId = 1,
                    DateSince = new DateTime(DateTime.Now.Year, 3, 10),
                    DateUntil = new DateTime(DateTime.Now.Year, 3, 10),
                    NumberOfHours = 8
                }
            };

            var vacationPackage = new VacationPackage { Id = 1, Name = "Standard", GrantedDays = 26, Year = 2025 };

            var canTakeVacation = _calculationService.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            _log.InfoWithConsole($"[Green]Employee \"{employee.Name}\" can take vacation: {canTakeVacation.ToString()}.[/]");

            return canTakeVacation;
        }
    }
}
