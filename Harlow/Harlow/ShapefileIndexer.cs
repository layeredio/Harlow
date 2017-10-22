using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harlow
{
    public abstract class ShapefileIndexer
    {
        protected int _Version;
        protected ShapeType _ShapeType;
        protected string _Filename;
        protected int _IndexFileSize;
        protected double[] _BBox;
        protected int[] _OffsetOfRecord;
        protected int[] _LengthOfRecord;
        protected int _FeatureCount;
		protected int _CoordinatePrecision = -1;

		public int RequiredPrecision { get { return _CoordinatePrecision; } set { _CoordinatePrecision = value; } }

		public ShapefileIndexer(string filename)
        {
            this._Filename = filename;
            _BBox = new double[4];
            ParseHeader(filename);// .shx file
            ReadIndex(filename); // .shx file
		}

		protected double ComputePrecision(double number)
		{
			double retVal = number;
			if (_CoordinatePrecision != -1) {
				retVal = Math.Round(number, _CoordinatePrecision);
			}

			return retVal;
		}

		/// <summary>
		/// Reads the header of the .shx index file and extracts the 
		/// information that is needed to read the remainder of the
		/// .shx file and the .shp file.
		/// </summary>
		private void ParseHeader(string filename)
        {
            filename = filename.Remove(filename.Length - 4, 4);
            filename += ".shx";

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            int i;
            int[] header_a = new int[9];
            double[] header_b = new double[8];

            //Read the first part of the header as integers
            for (i = 0; i < 9; ++i)
            {
                header_a[i] = br.ReadInt32();
            }

            //Read the second part of the header as doubles
            for (i = 0; i < 8; ++i)
            {
                header_b[i] = ComputePrecision(br.ReadDouble());
            }

            br.Close();

            //File size is reported in the header as a big endian
            //16 bit word.
            //Translating the size to 8 bit little endian.
            _IndexFileSize = 2 * SwitchByteOrder(header_a[6]);
            _Version = header_a[7];
            _ShapeType = (ShapeType)header_a[8]; //cast int to enum
            _BBox[0] = header_b[0];
            _BBox[1] = header_b[1];
            _BBox[2] = header_b[2];
            _BBox[3] = header_b[3];
        }

        /// <summary>
        /// Reads the record offset and length from the .shx index file and
        /// places the information into arrays (_OffsetOfRecord &
        /// _LengthOfRecord)
        /// </summary>
        private void ReadIndex(string filename)
        {
            filename = filename.Remove(filename.Length - 4, 4);
            filename += ".shx";

            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            int ibuffer;

            _FeatureCount = (_IndexFileSize - 100) / 8;

            _OffsetOfRecord = new int[_FeatureCount];
            _LengthOfRecord = new int[_FeatureCount];

            fs.Seek(100, 0);  //seek past the file header

            for (int x = 0; x < _FeatureCount; ++x)
            {
                //value must be multiplied by 2 to convert from the native
                //16 bit value (word) to an 8 bit value, and convert the
                //format from big endian to little endian.
                ibuffer = br.ReadInt32();
                _OffsetOfRecord[x] = 2 * SwitchByteOrder(ibuffer);

                //Add 8 bytes to the length to compensate for the
                //header at the beginning of each record in the shp file.
                ibuffer = br.ReadInt32();
                _LengthOfRecord[x] = (2 * SwitchByteOrder(ibuffer)) + 8;
            }

            br.Close();
        }

        ///<summary>
        ///Reverses the byte order of an integer (int32 only) 
        ///(big ->little or little ->big)
        ///</summary>
        protected int SwitchByteOrder(int i)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(i);
            Array.Reverse(buffer, 0, buffer.Length);

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
