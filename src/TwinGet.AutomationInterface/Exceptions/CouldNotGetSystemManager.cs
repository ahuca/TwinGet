// This file is licensed to you under MIT license.

namespace TwinGet.AutomationInterface.Exceptions
{
    [Serializable]
    public class CouldNotGetSystemManager : Exception
    {
        public CouldNotGetSystemManager()
        {
        }

        public CouldNotGetSystemManager(string message) : base(message)
        {
        }

        public CouldNotGetSystemManager(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
