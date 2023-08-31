using System;
using Stride.Games.AvaloniaDesktop;
using Stride.Graphics.Avalonia;

namespace Stride.Games;
internal class GameContextAvalonia : GameContext<EmbeddedAvaloniaWindow>
{
    public GameContextAvalonia(EmbeddedAvaloniaWindow control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false) 
        : base(control ?? new EmbeddedAvaloniaWindow(), requestedWidth, requestedHeight, isUserManagingRun)
    {
        ContextType = AppContextType.AvaloniaDesktop;
    }
}
