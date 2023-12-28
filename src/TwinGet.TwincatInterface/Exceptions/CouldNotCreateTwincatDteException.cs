// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions
{
    [Serializable]
    public class CouldNotCreateTwincatDteException : Exception
    {
        public CouldNotCreateTwincatDteException() { }

        public CouldNotCreateTwincatDteException(string messsage)
            : base(messsage) { }

        public CouldNotCreateTwincatDteException(string message, Exception inner)
            : base(message, inner) { }
    }
}
