using System;
using System.IO;
using System.Text;

namespace Harlow
{
    /// <summary>
    /// The class that provides access to the .dbf database file.
    /// </summary>
    internal class DbaseReader : DbaseIndexer
    {

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filename"></param>
        public DbaseReader(string filename) : base(filename) {}

        /// <summary>
        /// Get a string array that represents a record, one string for
        /// each field in the record.
        /// </summary>
        /// @return A string array that represents one record.
        public string[] this[int index]
		{
            get {
                return GetRecord(index);
            }
        }

		/// <summary>
		/// 
		/// </summary>
        public void Dispose()
		{
		}

		/// <summary>
		/// Get a whole record from the db file.  TODO: handle value types
		/// other than characters correctly.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		private string[] GetRecord(int index)
		{
			OpenBinaryReader();

			string[] buffer = new string[_FieldCount];

			if (index < _RecordCount)
			{
				_fs.Seek(_RecordStart + (index * _RecordLength), 0);
			}
			else
			{
				return null;
			}

			_br.ReadByte();  // delete flag

			for (int a = 0; a < _FieldCount; ++a)
			{
			
				switch (_FieldTypes[a])
				{
					case DbFieldType.C:
					case DbFieldType.c:
					case DbFieldType.D:
					case DbFieldType.d:
					case DbFieldType.F:
					case DbFieldType.f:
					case DbFieldType.L:
					case DbFieldType.l:
					case DbFieldType.N:
					case DbFieldType.n:
					default:
						buffer[a] = ReadStringFromStream(_br, _FieldLengths[a]);
						break;
				}
			}

			_br.Close();

			return buffer;
		}

		private string ReadStringFromStream(BinaryReader br, int bufferLength) {
            // Has to be read as byte and converted to char
            // because a 0x9e is appended to the end of some
            // field descriptors and it throws off br.ReadChars( x )
			char[] buffer = new char[bufferLength];

			for(int i = 0; i < bufferLength; i++)
				buffer[i] = (char)br.ReadByte();

			return new string(buffer);;
		}
    }
}
