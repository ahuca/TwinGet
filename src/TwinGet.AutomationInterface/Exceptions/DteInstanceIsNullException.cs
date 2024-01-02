// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Exceptions
{
    [Serializable]
    public class DteInstanceIsNullException : Exception
    {
        public DteInstanceIsNullException()
        {
        }

        public DteInstanceIsNullException(string message) : base(message)
        {
        }

        public DteInstanceIsNullException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
