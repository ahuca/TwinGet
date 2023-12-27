// This file is licensed to you under MIT license.

namespace Test.Utils
{
    [Serializable]
    public class TestDirectoryCreationException : Exception
    {
        public TestDirectoryCreationException() : base() { }
        public TestDirectoryCreationException(string msg) : base(msg) { }
    }
}
