using System;
using BlackCoat;
using SFML.Window;

namespace DreamAwake
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
#if !DEBUG
            var launcher = new Launcher()
            {
                //BannerImage = Image.FromFile("Assets\\Banner.png"),
                Text = Game.TITLE
            };
            var device = Device.Create(launcher, Game.TITLE);
            if (device == null) return;
#endif

#if DEBUG
            var vm = new VideoMode(800, 600);
            var device = Device.Create(vm, Game.TITLE, Styles.Default, 0, false, 120);
#endif
            using (var core = new Core(device))
            {
#if DEBUG
                core.Debug = true;
#endif
                // Setup Scene / Level Management
                Game.Core = core;

                //core.SceneManager.ChangeScene(new BlackCoatIntro(core, new MainMenuScene(core)));
                core.SceneManager.ChangeScene(new MainMenuScene(core));
                core.Run();
            }
        }
    }
}