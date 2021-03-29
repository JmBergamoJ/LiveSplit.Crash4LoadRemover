using System;
using System.Runtime.InteropServices;

namespace LiveSplit.Crash4LoadRemover.Memory.Reader
{
    [StructLayout(LayoutKind.Sequential)]
	public struct MemInfo
	{
		public IntPtr BaseAddress;
		public IntPtr AllocationBase;
		public uint AllocationProtect;
		public IntPtr RegionSize;
		public uint State;
		public uint Protect;
		public uint Type;
		public override string ToString()
		{
			return BaseAddress.ToString("X") + " " + Protect.ToString("X") + " " + State.ToString("X") + " " + Type.ToString("X") + " " + RegionSize.ToString("X");
		}
	}
}
