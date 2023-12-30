// This file is licensed to you under MIT license.

namespace TwinGet.TwincatInterface.Exceptions;

[Serializable]
public class TwincatInterfaceException : Exception
{
    public TwincatInterfaceException() { }

    public TwincatInterfaceException(string msg)
        : base(msg) { }

    public TwincatInterfaceException(string msg, Exception inner)
        : base(msg, inner) { }

    public TwincatInterfaceException(string msg, string helpLink)
        : base(msg)
    {
        HelpLink = helpLink;
    }
}
