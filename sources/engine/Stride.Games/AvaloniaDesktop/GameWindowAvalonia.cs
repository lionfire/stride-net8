
using Avalonia.Controls;
using Stride.Core.Mathematics;
using Stride.Graphics;

namespace Stride.Games.AvaloniaDesktop;
public class GameWindowAvalonia : GameWindow<NativeControlHost>
{
    public override bool AllowUserResizing { get; set; }
    public override Rectangle ClientBounds { get; }
    public override DisplayOrientation CurrentOrientation { get; }
    public override bool IsMinimized { get; }
    public override bool Focused { get; }
    public override bool IsMouseVisible { get; set; }
    public override WindowHandle NativeWindow { get; }
    public override bool Visible { get; set; }
    public override bool IsBorderLess { get; set; }

    public override void BeginScreenDeviceChange(bool willBeFullScreen)
    {
        throw new System.NotImplementedException();
    }

    public override void EndScreenDeviceChange(int clientWidth, int clientHeight)
    {
        throw new System.NotImplementedException();
    }

    protected override void Initialize(GameContext<NativeControlHost> context)
    {
        throw new System.NotImplementedException();
    }

    protected override void SetTitle(string title)
    {
        throw new System.NotImplementedException();
    }

    protected internal override void SetSupportedOrientations(DisplayOrientation orientations)
    {
        throw new System.NotImplementedException();
    }

    internal override void Resize(int width, int height)
    {
        throw new System.NotImplementedException();
    }

    internal override void Run()
    {
        throw new System.NotImplementedException();
    }
}
