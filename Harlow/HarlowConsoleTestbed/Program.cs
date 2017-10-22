using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HarlowConsoleTestbed {
	class Program {
		static void Main(string[] args)
		{
			try{
				Console.WriteLine("Starting Harlow interactions.");
			} finally{
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey();
			}
		}
	}
}
