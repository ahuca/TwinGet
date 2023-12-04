// This file is licensed to you under MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwinGet.AutomationInterface.Test
{
    internal static class TestTwincatSolutionConstants
    {
        public static readonly string s_testTwincatSolutionRootFolder = Path.Join(@$"{AppDomain.CurrentDomain.BaseDirectory}", @"TestTwincatProject");
        public static readonly string s_testTwincatSolution = Path.Join(@$"{AppDomain.CurrentDomain.BaseDirectory}", @"TestTwincatProject\TestTwincatProject.sln");
    }
}
