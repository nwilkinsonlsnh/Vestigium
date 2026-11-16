using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace Vestigium.Themes.Tests;

internal static class PackScheme
{
    private static readonly ManualResetEventSlim Ready = new(false);

    [ModuleInitializer]
    internal static void Register()
    {
        if (Application.Current != null)
        {
            Ready.Set();
            return;
        }

        var thread = new Thread(() =>
        {
            var app = new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            Ready.Set();
            Dispatcher.Run();
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.IsBackground = true;
        thread.Start();
        if (!Ready.Wait(TimeSpan.FromSeconds(10)))
            throw new TimeoutException("WPF Application did not start.");
    }

    public static void OnApplication(Action action)
    {
        var app = Application.Current ?? throw new InvalidOperationException("WPF Application was not started.");
        if (app.Dispatcher.CheckAccess())
            action();
        else
            app.Dispatcher.Invoke(action);
    }
}
