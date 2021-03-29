using LiveSplit.Crash4LoadRemover.Memory.Reader;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Crash4LoadRemover.Memory
{
	public class GamePointer<T> : IGamePointer where T : struct, IEquatable<T>
	{
		private int[] offsets;
		private T currentValue;

		public GamePointer(string name, bool refreshEnabled, params int[] offsets)
		{
			this.offsets = offsets;

			Name = name;
			IsRefreshEnabled = refreshEnabled;
		}

		// Setting the process publicly is easier than passing it into functions repeatedly.
		public Process Process { get; set; }

		public bool IsRefreshEnabled { get; set; }
		public bool IsPointerValid { get; private set; }

		public string Name { get; }

		public event Action<T, T> OnValueChange;

		public void Validate()
		{
			try
			{
				Read();
			}
			catch (Exception e)
			{
				Logging.Write(e.ToString());
				IsPointerValid = false;
			}

			IsPointerValid = true;
		}

		public T Read()
		{
			return Process.Read<T>(Process.MainModule.BaseAddress, offsets);
		}

		public void Refresh()
		{
			T newValue = Read();

			if (!newValue.Equals(currentValue))
			{
				OnValueChange?.Invoke(currentValue, newValue);
				currentValue = newValue;
			}
		}
	}
}
