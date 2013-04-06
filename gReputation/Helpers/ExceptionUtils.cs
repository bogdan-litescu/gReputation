using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace gReputation.Helpers
{
    public static class ExceptionUtils
    {
        public static string FlattenException(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null) {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.GetType() + ": " + exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static string Flatten(this Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null) {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.GetType() + ": " + exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }
    }
}