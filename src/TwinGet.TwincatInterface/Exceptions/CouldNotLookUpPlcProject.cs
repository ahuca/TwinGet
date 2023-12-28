// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions
{
    [Serializable]
    public class CouldNotLookUpPlcProject : Exception
    {
        public string? Name { get; }

        public CouldNotLookUpPlcProject() { }

        public CouldNotLookUpPlcProject(string message)
            : base(message) { }

        public CouldNotLookUpPlcProject(string message, Exception inner)
            : base(message, inner) { }

        public CouldNotLookUpPlcProject(string message, string name)
            : this(message)
        {
            Name = name;
        }
    }
}
