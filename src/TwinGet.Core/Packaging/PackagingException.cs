// This file is licensed to you under MIT license.

namespace TwinGet.Core.Packaging
{
    public class PackagingException : Exception
    {
        public PackagingException()
        {
        }

        public PackagingException(string messsage) : base(messsage)
        {
        }

        public PackagingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
