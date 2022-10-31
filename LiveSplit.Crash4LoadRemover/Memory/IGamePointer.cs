using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Crash4LoadRemover.Memory
{
	// This inteface is a convenience in order to group pointers together in the memory class. It's basically just a subset of
	// GamePointer functionality.
	public interface IGamePointer
	{
		Process Process { get; set; }
		bool IsRefreshEnabled { get; set; }
		bool IsPointerValid { get; }
		string Name { get; }
        string ModuleName { get; }
        void Validate();
		void Refresh();
	}
}
