using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harlow;
using Newtonsoft.Json;

namespace HarlowConsoleTestbed {
	class Program {
		static void Main(string[] args)
		{
			try{
				Console.WriteLine("Starting Harlow interactions.");

				string shapeFile = @"D:\Data\geo\tiger\TIGER2016\STATE\tl_2016_us_state.shp";
				ShapeFileReader reader = new ShapeFileReader(shapeFile);
				reader.RequiredPrecision = 6;
				reader.LoadFile();

				using( StreamWriter sw = new StreamWriter(@"C:\Projects\Layered.io\Data\JSON\HarlowFeatures\us_states_features_temp.txt")){
			//		sw.WriteLine("[");
					foreach (var f in reader.Features) {
						string json = JsonConvert.SerializeObject(f);
			//			var fe = JsonConvert.DeserializeObject<VectorShape>(json);

						sw.WriteLine(json);
			//			sw.WriteLine(",");
					}
			//		sw.WriteLine("]");
				}

			} finally {
				Console.WriteLine("Press any key to exit.");
				Console.ReadKey();
			}
		}
	}
}
