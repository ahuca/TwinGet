// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions
{
    [Serializable]
    public class InvalidProjectFileFormat : FormatException
    {
        public string? Path { get; }

        public InvalidProjectFileFormat() { }

        public InvalidProjectFileFormat(string message)
            : base(message) { }

        public InvalidProjectFileFormat(string message, Exception inner)
            : base(message, inner) { }

        public InvalidProjectFileFormat(string message, string filePath)
            : this(message)
        {
            Path = filePath;
        }
    }
}
