// This file is licensed to you under MIT license.


using System.Text;
using Microsoft.Extensions.Logging;

namespace TwinGet.Core.Packaging;

public partial class PackagingException
{
    public object[]? Args { get; }

    public PackagingException(string messsage, params object[]? args)
        : base(messsage)
    {
        Errors = new Dictionary<string, string[]>();
        Args = args;
    }
}

public static class PackagingExceptionExtensions
{
    public static bool LogWith(this PackagingException exception, ILogger? logger)
    {
        if (logger is not null)
        {
            StringBuilder stringBuilder = new(exception.AsLogMessage());
            //logger.LogError(exception.AsLogMessage());
            if (!string.IsNullOrEmpty(exception.HelpLink))
            {
                stringBuilder.AppendLine(exception.HelpLink);
                //logger.LogDebug(exception.HelpLink);
            }
            stringBuilder.AppendLine(exception.StackTrace);
            //logger.LogDebug(exception.StackTrace);
            logger.LogDebug(stringBuilder.ToString());

            return true;
        }

        return false;
    }
}
