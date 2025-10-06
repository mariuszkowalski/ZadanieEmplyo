using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers
{
    public static class ArgumentValidation
    {
        public static T CheckIfNotNull<T>(this T value, string paramName, ILogger logger)
        {
            if (value is null)
            {
                var ex = new ArgumentNullException(paramName, $"Argument {paramName} cannot be null.");

                logger.LogError(ex, $"Argument Validation FAILED: argument {paramName} is null.");

                throw ex;
            }
            return value;
        }

    }
}
