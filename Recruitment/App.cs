using DataAccess.Models;
using Microsoft.Extensions.Logging;
using Services.Core.DataOperation;
using Services.Helpers;
using Services.Questions;
using Services.Structure;
using Spectre.Console;
using static System.Net.Mime.MediaTypeNames;


public class App
{
    private readonly ILogger<App> _log;
    private readonly DatabaseInitializerService _dbService;
    private readonly IQuestionsService _questionsService;

    public App(ILogger<App> log, DatabaseInitializerService dbService, IQuestionsService questionsService)
    {
        _log = log;
        _dbService = dbService;
        _questionsService = questionsService;
    }

    public void Run()
    {
        _log.InfoWithConsole("[Green]Application started.[/]");

        AnsiConsole.MarkupLine("[Green]Application started.[/]");

        var seedData = AnsiConsole.Prompt(
            new TextPrompt<bool>("[Red]WARNING!!! Required on first launch.[/] Initialize database with seed data?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(false)
                .WithConverter(choice => choice ? "y" : "n"));
        Console.WriteLine(seedData ? "yes" : "no");



        _dbService.InitializeDatabase(seedData);

        var exit = false;
        while (!exit)
        {
            try
            {
                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("\n[green]Select problem:[/]")
                    .AddChoices(new[]
                    {
                        "Problem 1. Employee Structure Example.",
                        "Problem 3. Calculate Remaining Vacation Days.",
                        "Problem 4. Check If Employee Can Take Vacation.",
                        "Exit"
                    }));

                switch (choice)
                {
                    case "Problem 1. Employee Structure Example.":
                        // Problem 1.
                        _questionsService.EmployeeStructureExample();
                        break;

                    case "Problem 3. Calculate Remaining Vacation Days.":
                        // Problem 3.
                        _questionsService.CalculateRemainingVacationDaysExample();
                        break;

                    case "Problem 4. Check If Employee Can Take Vacation.":
                        // Problem 4.
                        _questionsService.CheckIfEmployeeCanTakeVacationExample();
                        break;

                    case "Exit":
                        exit = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Unhandled exception occurred in application. Details in log file.[/]");
                _log.LogError($"{ex} Unhandled exception occurred in application.");
            }

        }

        _log.InfoWithConsole("[Green]Application finished.[/]");
    }
}