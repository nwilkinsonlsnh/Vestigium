using System.IO.Packaging;
using System.Runtime.CompilerServices;

namespace Vestigium.Themes.Tests;

internal static class PackScheme
{
    [ModuleInitializer]
    internal static void Register()
    {
        // Registers pack:// without creating an Application.
        // WPF allows only one Application per AppDomain.
        _ = PackUriHelper.UriSchemePack;
    }
}
