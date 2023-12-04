// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface.Exceptions
{
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
