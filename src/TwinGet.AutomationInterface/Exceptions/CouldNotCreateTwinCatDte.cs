// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface.Exceptions
{
    internal class CouldNotCreateTwinCatDte : Exception
    {
        public CouldNotCreateTwinCatDte()
        {
        }

        public CouldNotCreateTwinCatDte(string messsage) : base(messsage)
        {
        }

        public CouldNotCreateTwinCatDte(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
