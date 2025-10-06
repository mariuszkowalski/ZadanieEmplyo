using Microsoft.Extensions.Logging;
using Spectre.Console;
using System.Text.RegularExpressions;

namespace Services.Helpers
{
    public static class LoggerExtensions
    {
        public static void InfoWithConsole(this ILogger logger, string message)
        {
            logger.LogInformation(message);
            AnsiConsole.MarkupLine($"[green]{message}[/]");
        }

        public static void ErrorWithConsole(this ILogger logger, string message)
        {
            logger.LogError(message);
            AnsiConsole.MarkupLine($"[red]{message}[/]");
        }

        public static void WarnWithConsole(this ILogger logger, string message)
        {
            logger.LogWarning(message);
            AnsiConsole.MarkupLine($"[yellow]{message}[/]");
        }
    }
}