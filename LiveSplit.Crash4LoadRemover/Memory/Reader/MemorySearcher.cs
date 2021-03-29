using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace LiveSplit.Crash4LoadRemover.Memory.Reader
{
    public class MemorySearcher
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MemInfo lpBuffer, int dwLength);

		public List<MemInfo> memoryInfo;
		public Func<MemInfo, bool> MemoryFilter = delegate (MemInfo info) {
			return (info.State & 0x1000) != 0 && (info.Protect & 0x100) == 0;
		};

		public byte[] ReadMemory(Process process, int index)
		{
			MemInfo info = memoryInfo[index];
			byte[] buff = new byte[(uint)info.RegionSize];
			ReadProcessMemory(process.Handle, info.BaseAddress, buff, (uint)info.RegionSize, 0);
			return buff;
		}
		public IntPtr FindSignature(Process process, string signature)
		{
			byte[] pattern;
			bool[] mask;
			GetSignature(signature, out pattern, out mask);
			GetMemoryInfo(process.Handle);
			int[] offsets = GetCharacterOffsets(pattern, mask);

			for (int i = 0; i < memoryInfo.Count; i++)
			{
				byte[] buff = ReadMemory(process, i);
				MemInfo info = memoryInfo[i];

				int result = ScanMemory(buff, pattern, mask, offsets);
				if (result != int.MinValue)
				{
					return info.BaseAddress + result;
				}
			}

			return IntPtr.Zero;
		}
		public List<IntPtr> FindSignatures(Process process, string signature)
		{
			byte[] pattern;
			bool[] mask;
			GetSignature(signature, out pattern, out mask);
			GetMemoryInfo(process.Handle);
			int[] offsets = GetCharacterOffsets(pattern, mask);

			List<IntPtr> pointers = new List<IntPtr>();
			for (int i = 0; i < memoryInfo.Count; i++)
			{
				byte[] buff = ReadMemory(process, i);
				MemInfo info = memoryInfo[i];

				ScanMemory(pointers, info, buff, pattern, mask, offsets);
			}
			return pointers;
		}

		// This function searches for the given array of signatures in order. It's an all or nothing algorithm, so the function will either
		// find pointers for all signatures or return null if any weren't found in the specific order given.
		public IntPtr[] FindSignatures(Process process, string[] signatures)
		{
			GetMemoryInfo(process.Handle);

			IntPtr[] pointers = new IntPtr[signatures.Length];
			int index = 0;

			GetSignature(signatures[index], out byte[] pattern, out bool[] mask);
			int[] offsets = GetCharacterOffsets(pattern, mask);

			for (int i = 0; i < memoryInfo.Count; i++)
			{
				byte[] buffer = ReadMemory(process, i);

				MemInfo info = memoryInfo[i];

				int current = 0;
				int end = pattern.Length - 1;

				while (current <= buffer.Length - pattern.Length)
				{
					for (int j = end; buffer[current + j] == pattern[j] || mask[j]; j--)
					{
						if (j == 0)
						{
							pointers[index] = info.BaseAddress + current;

							if (index == signatures.Length - 1)
							{
								return pointers;
							}

							index++;
							GetSignature(signatures[index], out pattern, out mask);
							offsets = GetCharacterOffsets(pattern, mask);
							current += j + 1;
							end = pattern.Length - 1;

							break;
						}
					}

					int offset = offsets[buffer[current + end]];

					current += offset;
				}
			}

			return null;
		}

		public void GetMemoryInfo(IntPtr pHandle)
		{
			if (memoryInfo != null) { return; }

			memoryInfo = new List<MemInfo>();
			IntPtr current = (IntPtr)65536;
			while (true)
			{
				MemInfo memInfo = new MemInfo();
				int dump = VirtualQueryEx(pHandle, current, out memInfo, Marshal.SizeOf(memInfo));
				if (dump == 0)
				{
					break;
				}

				long regionSize = (long)memInfo.RegionSize;
				if (regionSize <= 0 || (int)regionSize != regionSize)
				{
					if (MemoryReader.is64Bit)
					{
						current = (IntPtr)((ulong)memInfo.BaseAddress + (ulong)memInfo.RegionSize);
						continue;
					}
					break;
				}

				if (MemoryFilter(memInfo))
				{
					memoryInfo.Add(memInfo);
				}

				current = memInfo.BaseAddress + (int)regionSize;
			}
		}
		private int ScanMemory(byte[] data, byte[] search, bool[] mask, int[] offsets)
		{
			int current = 0;
			int end = search.Length - 1;
			while (current <= data.Length - search.Length)
			{
				for (int i = end; data[current + i] == search[i] || mask[i]; i--)
				{
					if (i == 0)
					{
						return current;
					}
				}
				int offset = offsets[data[current + end]];
				current += offset;
			}

			return int.MinValue;
		}
		private void ScanMemory(List<IntPtr> pointers, MemInfo info, byte[] data, byte[] search, bool[] mask, int[] offsets)
		{
			int current = 0;
			int end = search.Length - 1;
			while (current <= data.Length - search.Length)
			{
				for (int i = end; data[current + i] == search[i] || mask[i]; i--)
				{
					if (i == 0)
					{
						pointers.Add(info.BaseAddress + current);
						break;
					}
				}
				int offset = offsets[data[current + end]];
				current += offset;
			}
		}
		private int[] GetCharacterOffsets(byte[] search, bool[] mask)
		{
			int[] offsets = new int[256];
			int unknown = 0;
			int end = search.Length - 1;
			for (int i = 0; i < end; i++)
			{
				if (!mask[i])
				{
					offsets[search[i]] = end - i;
				}
				else
				{
					unknown = end - i;
				}
			}

			if (unknown == 0)
			{
				unknown = search.Length;
			}

			for (int i = 0; i < 256; i++)
			{
				int offset = offsets[i];
				if (unknown < offset || offset == 0)
				{
					offsets[i] = unknown;
				}
			}
			return offsets;
		}
		private void GetSignature(string searchString, out byte[] pattern, out bool[] mask)
		{
			int length = searchString.Length >> 1;
			pattern = new byte[length];
			mask = new bool[length];

			length <<= 1;
			for (int i = 0, j = 0; i < length; i++)
			{
				byte temp = (byte)(((int)searchString[i] - 0x30) & 0x1F);
				pattern[j] |= temp > 0x09 ? (byte)(temp - 7) : temp;
				if (searchString[i] == '?')
				{
					mask[j] = true;
					pattern[j] = 0;
				}
				if ((i & 1) == 1)
				{
					j++;
				}
				else
				{
					pattern[j] <<= 4;
				}
			}
		}
	}
}
