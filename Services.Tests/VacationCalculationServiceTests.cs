using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using Services.Core.DataOperation.Models;
using Services.VacationCalculation;
using System;
using System.Collections.Generic;

namespace Services.Tests
{
    public class VacationCalculationServiceTests
    {
        private VacationCalculationService _service;
        private Mock<ILogger<VacationCalculationService>> _loggerMock;

        [SetUp]
        public void Setup()
        {
            _loggerMock = new Mock<ILogger<VacationCalculationService>>();
            _service = new VacationCalculationService(_loggerMock.Object);
        }

        [Test]
        public void employee_can_request_vacation()
        {
            var employee = new Employee { Id = 1, Name = "Mariusz Kowalski", TeamId = 1 };
            var vacationPackage = new VacationPackage { Id = 1, Name = "Standard", GrantedDays = 26, Year = 2025 };

            var vacations = new List<Vacation>
            {
                new() {
                    Id = 1,
                    EmployeeId = 1,
                    DateSince = new DateTime(DateTime.Now.Year, 1, 1),
                    DateUntil = new DateTime(DateTime.Now.Year, 1, 5),
                    NumberOfHours = 40
                }
            };

            var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void employee_cant_request_vacation()
        {
            var employee = new Employee { Id = 2, Name = "Jan Nowak", TeamId = 1 };
            var vacationPackage = new VacationPackage { Id = 1, Name="Micro", GrantedDays = 10, Year=2025 };

            var vacations = new List<Vacation>
            {
                new() {
                    Id = 1,
                    EmployeeId = 2,
                    DateSince = new DateTime(DateTime.Now.Year, 1, 1),
                    DateUntil = new DateTime(DateTime.Now.Year, 1, 10),
                    NumberOfHours = 80
                }
            };

            var result = _service.IfEmployeeCanRequestVacation(employee, vacations, vacationPackage);

            Assert.That(result, Is.False);
        }
    }
}
