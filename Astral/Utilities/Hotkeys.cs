using Astral.Models;
using Astral.Models.Configurations;
using SharpHook;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Astral.Utilities
{
    public class Hotkeys : IUtility, IStoppable, IConfiguredService<Models.Configurations.HotkeyConfig>
    {
        public HotkeyConfig Configuration { get; }

        private readonly TaskPoolGlobalHook hook =
            new TaskPoolGlobalHook();

        private readonly AstralStatus astralStatus;

        public Hotkeys(Models.AstralStatus astralStatus,
            Models.Configurations.HotkeyConfig hotkeyConfig)
        {
            this.astralStatus = astralStatus;
            Configuration = hotkeyConfig;

            hook.KeyPressed += KeyPressed;
            hook.KeyReleased += KeyReleased;

            _ = hook.RunAsync();
        }

        private List<SharpHook.Native.KeyCode> pressingCodes = new();

        public bool IsKeyPressed(SharpHook.Native.KeyCode key) =>
            pressingCodes.Contains(key);

        private void KeyReleased(object? sender, KeyboardHookEventArgs e)
        {
            while (pressingCodes.Contains(e.Data.KeyCode))
                pressingCodes.Remove(e.Data.KeyCode);

            switch (e.Data.KeyCode)
            {
                // Wtf
                case var value when value == Configuration.PauseInputs:
                    astralStatus.ResumePredictions();
                    break;
            }
        }

        private void KeyPressed(object? sender, KeyboardHookEventArgs e)
        {
            pressingCodes.Add(e.Data.KeyCode);

            switch (e.Data.KeyCode)
            {
                // Wtf
                case var value when value == Configuration.PauseInputs:
                    astralStatus.PausePredictions();
                    break;
            }
        }


        public void Stop()
        {
            hook.Dispose();
        }
    }
}
