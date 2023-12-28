// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions
{
    [Serializable]
    public class NotATwincatProject : Exception
    {
        public NotATwincatProject() { }

        public NotATwincatProject(string message)
            : base(message) { }

        public NotATwincatProject(string message, Exception inner)
            : base(message, inner) { }
    }
}
