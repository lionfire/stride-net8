using System.Runtime.InteropServices;
using Stride.Engine;
using Stride.Games;

namespace ParticlesSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // Create SDL window using panel
            var _sdlWindow = new Stride.Graphics.SDL.Window("Embedded Stride Window", panel1.Handle);
            var context = new GameContextSDL(_sdlWindow);

            // Start the game
            Game _game = new();
            Task.Factory.StartNew(() =>
            {
                // Must move this off current thread or the form will hang.
                _game.Run(context);
            }, TaskCreationOptions.LongRunning);
        }
    }
}
