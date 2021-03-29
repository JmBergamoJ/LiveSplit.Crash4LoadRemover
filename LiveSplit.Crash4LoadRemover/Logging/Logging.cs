using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveSplit.Crash4LoadRemover
{
	public static class Logging
	{
		public static void Write(string value)
		{
			if (Console.IsOutputRedirected)
			{
				using (StreamWriter writer = new StreamWriter("_Crash4LR.log", true))
				{
					writer.WriteLine(value);
				}
			}
			else
			{
				Console.WriteLine(value);
			}
		}
	}
}
