using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Crash4LoadRemover.Memory
{
    public class CrashMemory : GameMemory
    {
        private IGamePointer[] pointers;

        public CrashMemory() : base("CrashBandicoot4")
        {
            #region Memory Pointers

            #region Release Version
            //Loading = new GamePointer<byte>("Loading", true, 0x03C34B70, 0x718); //First part of the loading screen, showing the name of the level and such
            //Swirl = new GamePointer<byte>("Swirl", true, 0x0416E510, 0x7C0, 0xC0, 0x2F8); //Second part of the loading screen, showing the blue swirl
            //The memory value for Swirl also interacts with other elements in the game sometimes but it's not an issue here because both values have to be combined to count as a load
            // Pointers are ordered alphabetically to make logging a bit nicer. There's no performance difference regardless.
            #endregion

            #region Patch 1 - 1.1.04062021+
            //Loading = new GamePointer<byte>("Loading", true, 0x041A1538, 0xB0);
            //Swirl = new GamePointer<byte>("Swirl", true, 0x041883A0, 0x7C0, 0xC0, 0x2F8);
            #endregion

            #region Steam - Release
            //Loading = new GamePointer<byte>("Loading", "RTWorkQ.DLL", true, 0x26950);
            //Swirl = new GamePointer<byte>("Swirl", null, true, 0x043EF190, 0x7C0, 0x31C);
            #endregion

            #region Steam - Fix
            Loading = new GamePointer<byte>("Loading", null, true, 0x043C41F0, 0xA8);
            Swirl = new GamePointer<byte>("Swirl", null, true, 0x043EF190, 0x7C0, 0x31C);
            #endregion

            #region BattleNet - Fix
            //Loading = new GamePointer<byte>("Loading", null, true, 0x04058F3C);
            //Swirl = new GamePointer<byte>("Swirl", null, true, 0x041883A0, 0x7C0, 0xC0, 0x2F8);
            #endregion

            #endregion

            pointers = new IGamePointer[]
            {
                Loading,
                Swirl,
            };
        }

        public GamePointer<byte> Loading { get; }
        public GamePointer<byte> Swirl { get; }

        protected override void OnHook(Process process)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"[Memory] Process hooked ({DateTime.Now}).");

            for (int i = 0; i < pointers.Length; i++)
            {
                IGamePointer p = pointers[i];
                p.Process = process;
                p.Validate();

                string value = $"[Memory] {p.Name} pointer {(p.IsPointerValid ? "valid" : "invalid")}.";

                if (i == pointers.Length - 1)
                {
                    builder.Append(value);
                }
                else
                {
                    builder.AppendLine(value);
                }

                if (!p.IsPointerValid)
                {
                    p.Process = Process = null; //reset the process to hook again because we need all the pointers to be working
                }
            }

            Logging.Write(builder.ToString());
        }

        protected override void OnUnhook()
        {
            Logging.Write("[Memory] Process unhooked.");

            foreach (IGamePointer p in pointers)
            {
                p.Process = null;
            }
        }

        public void Refresh()
        {
            foreach (IGamePointer p in pointers)
            {
                if (p.IsPointerValid && p.IsRefreshEnabled)
                {
                    p.Refresh();
                }
            }
        }
    }
}
