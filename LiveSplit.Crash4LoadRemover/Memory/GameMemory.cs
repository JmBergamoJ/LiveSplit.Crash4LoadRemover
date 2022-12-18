using LiveSplit.Crash4LoadRemover.Memory.Reader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Crash4LoadRemover.Memory
{
    public abstract class GameMemory
    {
		private readonly string processName;

		protected GameMemory(string processName)
		{
			this.processName = processName;
		}

		protected abstract void OnHook(Process process);
		protected abstract void OnUnhook();

		public bool ProcessHooked => Process != null && !Process.HasExited;

		public Process Process { get; protected set; }

		public bool HookProcess()
		{
			if (Process == null)
			{
				Process[] processes = Process.GetProcessesByName(processName);
				Process = processes.Length == 0 ? null : processes[0];

				if (Process == null || Process.HasExited)
				{
					return false;
				}

				MemoryReader.Update64Bit(Process);

				OnHook(Process);
			}
			else if (Process.HasExited)
			{
				Process = null;
				OnUnhook();

				return false;
			}

			return Process != null;
		}

	}
}
