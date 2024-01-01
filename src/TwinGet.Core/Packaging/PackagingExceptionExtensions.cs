// This file is licensed to you under MIT license.


using Microsoft.Extensions.Logging;

namespace TwinGet.Core.Packaging;

public static class PackagingExceptionExtensions
{
    public static bool LogWith(this PackagingException exception, ILogger? logger)
    {
        if (logger is not null)
        {
            logger.LogError(exception.AsLogMessage());
            if (!string.IsNullOrEmpty(exception.Source))
            {
                logger.LogError(exception.Source);
            }
            if (!string.IsNullOrEmpty(exception.HelpLink))
            {
                logger.LogError(exception.HelpLink);
            }
            logger.LogError(exception.StackTrace);

            return true;
        }

        return false;
    }
}
