// This file is licensed to you under MIT license.

using Microsoft.Extensions.Logging;

namespace TwinGet.Core.Commands
{
    public interface ITwinGetCommand

    {
        public ILogger Logger { get; set; }
    }
}
