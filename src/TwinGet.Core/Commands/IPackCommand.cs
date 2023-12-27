// This file is licensed to you under MIT license.

namespace TwinGet.Core.Commands
{
    public interface IPackCommand : ITwinGetCommand
    {
        public string Path { get; }
        public string Solution { get; }
        public string OutputDirectory { get; }
    }
}
