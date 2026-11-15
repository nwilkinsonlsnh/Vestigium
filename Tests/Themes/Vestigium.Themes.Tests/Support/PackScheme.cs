using System.Runtime.CompilerServices;
using System.Windows;

namespace Vestigium.Themes.Tests;

internal static class PackScheme
{
    [ModuleInitializer]
    internal static void Register()
    {
        if (UriParser.IsKnownScheme("pack")) return;

        Exception? error = null;
        var thread = new Thread(() =>
        {
            try
            {
                if (Application.Current == null)
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
