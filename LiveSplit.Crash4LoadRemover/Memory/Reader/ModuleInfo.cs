using System;
using System.Runtime.InteropServices;

namespace LiveSplit.Crash4LoadRemover.Memory.Reader
{
    [StructLayout(LayoutKind.Sequential)]
	public struct ModuleInfo
	{
		public IntPtr BaseAddress;
		public uint ModuleSize;
		public IntPtr EntryPoint;
	}
}
