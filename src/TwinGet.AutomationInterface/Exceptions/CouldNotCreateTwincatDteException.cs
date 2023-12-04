// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface.Exceptions
{
    public class CouldNotCreateTwincatDteException : Exception
    {
        public CouldNotCreateTwincatDteException()
        {
        }

        public CouldNotCreateTwincatDteException(string messsage) : base(messsage)
        {
        }

        public CouldNotCreateTwincatDteException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
