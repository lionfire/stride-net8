using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace Stride.Games;
internal class GameContextAvalonia : GameContext<NativeControlHost>
{
    public GameContextAvalonia(NativeControlHost control, int requestedWidth = 0, int requestedHeight = 0, bool isUserManagingRun = false) : base(control, requestedWidth, requestedHeight, isUserManagingRun)
    {
        ContextType = AppContextType.AvaloniaDesktop;
    }
}
