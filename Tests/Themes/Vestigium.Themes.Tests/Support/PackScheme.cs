using System.Runtime.CompilerServices;
using System.Windows;

namespace Vestigium.Themes.Tests;

internal static class PackScheme
{
    [ModuleInitializer]
    internal static void Register()
    {
        if (Application.Current != null) return;

        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                // One Application per AppDomain. Constructing it registers the pack:// web-request prefix.
                new Application { ShutdownMode = ShutdownMode.OnExplicitShutdown };
            }
            catch (Exception ex)
            {
                error = ex;
            }
        });
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();
        if (error is not null)
            throw error;
    }
}
