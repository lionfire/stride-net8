using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform;

namespace Stride.Graphics.Avalonia;
public class EmbeddedAvaloniaWindow : NativeControlHost
{
    public IntPtr WindowHandle;

    protected override IPlatformHandle CreateNativeControlCore(IPlatformHandle parent)
    {
        var piHandle = base.CreateNativeControlCore(parent);
        WindowHandle = piHandle.Handle;

        return base.CreateNativeControlCore(parent);
    }

    [SupportedOSPlatform("windows")]
    IPlatformHandle CreateWin32(IPlatformHandle parent)
    {
        throw new NotImplementedException();
    }

}
