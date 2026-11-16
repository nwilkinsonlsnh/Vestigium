using System.IO.Packaging;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Vestigium.Themes.Tests;

internal static class PackScheme
{
    private static readonly object Gate = new();
    private static Exception? _startError;

    [ModuleInitializer]
    internal static void RegisterScheme()
    {
        // Registers pack:// with the URI parser. Does not start WPF and does not wait on another thread.
        _ = PackUriHelper.UriSchemePack;
    }

    public static void OnApplication(Action action)
    {
        EnsureStarted();
        var app = Application.Current ?? throw new InvalidOperationException("WPF Application was not started.", _startError);
        if (app.Dispatcher.CheckAccess())
            action();
        else
            app.Dispatcher.Invoke(action);
    }

    private static void EnsureStarted()
    {
        if (Application.Current != null) return;

        lock (Gate)
        {
            if (Application.Current != null) return;

            using var ready = new ManualResetEventSlim(false);
            var thread = new Thread(() =>
            {
                try
                {
                    new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
                    ready.Set();
                    Dispatcher.Run();
                }
                catch (Exception ex)
                {
                    _startError = ex;
                    ready.Set();
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = true;
            thread.Start();
            if (!ready.Wait(TimeSpan.FromSeconds(15)))
                throw new TimeoutException("WPF Application did not start.");
            if (_startError is not null)
                throw new InvalidOperationException("WPF Application failed to start.", _startError);
        }
    }
}
