// This file is licensed to you under MIT license.

namespace TwinGet.Core.Commands
{
    public interface IPackCommand : ITwinGetCommand
    {
        public string Path { get; set; }
        public string Solution { get; set; }
        public string OutputDirectory { get; set; }
    }
}
