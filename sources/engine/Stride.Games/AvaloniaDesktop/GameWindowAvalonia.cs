
using System.Reflection.Metadata;
using Avalonia.Controls;
using Avalonia.Platform;
using Avalonia.VisualTree;
using ServiceWire;
using Stride.Core.Mathematics;
using Stride.Graphics;
using Stride.Graphics.Avalonia;

namespace Stride.Games.AvaloniaDesktop;
public class GameWindowAvalonia : GameWindow<EmbeddedAvaloniaWindow>
{
    public override bool AllowUserResizing { get; set; }
    public override Rectangle ClientBounds { get; }
    public override DisplayOrientation CurrentOrientation { get; }
    public override bool IsMinimized { get; }
    public override bool Focused { get; }
    public override bool IsMouseVisible { get; set; }
    public override WindowHandle NativeWindow
    {
        get
        {
            return windowHandle;
        }
    }
    private WindowHandle windowHandle;
    public override bool Visible { get; set; }
    public override bool IsBorderLess { get; set; }

    private WindowHandle _nativeWindowHandle;
    private EmbeddedAvaloniaWindow _nativeControl;

    public override void BeginScreenDeviceChange(bool willBeFullScreen)
    {

    }

    public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
    {

    }

    protected override void Initialize(GameContext<EmbeddedAvaloniaWindow> context)
    {
        _nativeControl = context.Control;

        windowHandle = new WindowHandle(AppContextType.AvaloniaDesktop, _nativeControl, _nativeControl.WindowHandle);

        _nativeControl.Width = context.RequestedWidth;
        _nativeControl.Height = context.RequestedHeight;
    }

    protected override void SetTitle(string title)
    {

    }

    protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
    {

    }

    internal override void Resize(int width, int height)
    {

    }

    internal override void Run()
    {

    }
}
