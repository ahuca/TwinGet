// This file is licensed to you under MIT license.

using System.Runtime.InteropServices;
using TwinGet.TwincatInterface.ComMessageFilter;
using TwinGet.TwincatInterface.Exceptions;
using TwinGet.TwincatInterface.Utils;

namespace TwinGet.TwincatInterface;

public class TwincatDteProvider : IDisposable
{
    private bool _disposedValue;
    private readonly bool _startedMsgFiltering = false;

    public object Owner { get; }
    public string ProgId { get; }
    public EnvDTE80.DTE2 Dte { get; }

    /// <summary>
    /// Construct the wrapper class for a TwinCAT DTE instance.
    /// </summary>
    /// <param name="owner">The object that created this object.</param>
    /// <param name="startMsgFiltering">Start message filtering <see cref="MessageFilter.Register()"/> if this is true. Default value is <code>true</code>.</param>
    /// <exception cref="TwincatInterfaceException">Throw this exception when it fails to create a TwinCAT DTE instance.</exception>
    public TwincatDteProvider(object owner, bool startMsgFiltering = true)
    {
        ArgumentNullException.ThrowIfNull(owner, nameof(owner));
        Owner = owner;

        foreach (string p in TwincatConstants.SupportedProgIds)
        {
            var t = Type.GetTypeFromProgID(p);

            if (t is null)
            {
                continue;
            }

            EnvDTE80.DTE2? dte;
            try
            {
                dte = (EnvDTE80.DTE2?)Activator.CreateInstance(t);
                if (dte is null)
                {
                    continue;
                }
            }
            catch
            {
                continue;
            }

            if (dte.IsTwinCatIntegrated())
            {
                ProgId = p;
                Dte = dte;
                break;
            }
        }

        if (Dte is null || string.IsNullOrEmpty(ProgId))
        {
            throw new TwincatInterfaceException(ExceptionStrings.CouldNotCreateTwincatDteInstance);
        }

        Dte.SuppressUI = true;
        Dte.MainWindow.Visible = false;
        Dte.UserControl = false;

        if (startMsgFiltering)
        {
            _startedMsgFiltering = true;
            MessageFilter.Register();
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing) { }

            if (Dte is not null)
            {
                Dte.Quit();
                Marshal.ReleaseComObject(Dte);

                if (_startedMsgFiltering)
                {
                    MessageFilter.Revoke();
                }
            }

            _disposedValue = true;
        }
    }

    ~TwincatDteProvider()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
