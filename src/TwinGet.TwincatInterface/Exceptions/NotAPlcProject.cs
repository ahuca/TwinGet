// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions
{
    [Serializable]
    public class NotAPlcProject : Exception
    {
        public NotAPlcProject()
        {
        }

        public NotAPlcProject(string message) : base(message)
        {
        }

        public NotAPlcProject(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
