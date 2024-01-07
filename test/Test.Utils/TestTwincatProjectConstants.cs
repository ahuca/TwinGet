// This file is licensed to you under MIT license.

namespace Test.Utils;

internal static class TestTwincatProjectConstants
{
    internal static readonly string s_testTwincatProject = Path.Join(
        @$"{AppDomain.CurrentDomain.BaseDirectory}",
        @"TestTwincatProject"
    );
    internal const string NonTwincatProjectNamePrefix = "TestNonTwincatProject";
    internal const string TwincatProjectNamePrefix = "TestTwincatProject";
}
