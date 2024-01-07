// This file is licensed to you under MIT license.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace TwinGet.TwincatInterface.ComMessageFilter;

/// <summary>
/// A COM message filtering class <see href="https://infosys.beckhoff.com/english.php?content=../content/1033/tc3_automationinterface/242727947.html&id=">
/// </summary>
[SupportedOSPlatform("windows")]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0018:Inline variable declaration",
    Justification = "<Pending>"
)]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0059:Unnecessary assignment of a value",
    Justification = "<Pending>"
)]
[System.Diagnostics.CodeAnalysis.SuppressMessage(
    "Style",
    "IDE0049:Simplify Names",
    Justification = "<Pending>"
)]
internal class MessageFilter : IOleMessageFilter
{
    public static void Register()
    {
        IOleMessageFilter newFilter = new MessageFilter();
        IOleMessageFilter? oldFilter = null;
        int test = CoRegisterMessageFilter(newFilter, out oldFilter);

        if (test != 0)
        {
            Console.WriteLine(
                string.Format("CoRegisterMessageFilter failed with error : {0}", test)
            );
        }
    }

    public static void Revoke()
    {
        IOleMessageFilter? oldFilter = null;
        int test = CoRegisterMessageFilter(null, out oldFilter);
    }

    int IOleMessageFilter.HandleInComingCall(
        int dwCallType,
        System.IntPtr hTaskCaller,
        int dwTickCount,
        System.IntPtr lpInterfaceInfo
    )
    {
        //returns the flag SERVERCALL_ISHANDLED.
        return 0;
    }

    int IOleMessageFilter.RetryRejectedCall(
        System.IntPtr hTaskCallee,
        int dwTickCount,
        int dwRejectType
    )
    {
        // Thread call was refused, try again.
        if (dwRejectType == 2)
        // flag = SERVERCALL_RETRYLATER.
        {
            // retry thread call at once, if return value >=0 &
            // <100.
            return 99;
        }
        return -1;
    }

    int IOleMessageFilter.MessagePending(
        System.IntPtr hTaskCallee,
        int dwTickCount,
        int dwPendingType
    )
    {
        //return flag PENDINGMSG_WAITDEFPROCESS.
        return 2;
    }

    // implement IOleMessageFilter interface.
    [DllImport("Ole32.dll")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage(
        "Interoperability",
        "SYSLIB1054:Use 'LibraryImportAttribute' instead of 'DllImportAttribute' to generate P/Invoke marshalling code at compile time",
        Justification = "<Pending>"
    )]
    private static extern int CoRegisterMessageFilter(
        IOleMessageFilter? newFilter,
        out IOleMessageFilter oldFilter
    );
}
