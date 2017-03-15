using System;
using System.IO;

namespace Harlow
{
    /// <summary>
    /// Loads the relevant data structures by reading the header of the .dbf
    /// This is an Abstract class and is not directly instantiated, it is 
    /// inherited by the Dbase class.
    /// </summary>
    abstract internal class DbaseIndexer
    {
        protected string[] _FieldNames;
        protected DbFieldType[] _FieldTypes;
        protected byte[] _FieldLengths;
        protected uint _RecordCount;
        protected ushort _RecordLength;
        protected uint _RecordStart;
        protected uint _FieldCount;
        protected string _Filename;

        private byte _DbType;
        private byte _UpdateYear;
        private byte _UpdateMonth;
        private byte _UpdateDay;
        private ushort _HeaderLength;

		protected BinaryReader _br;
		protected FileStream _fs;

        public DbaseIndexer(string filename)
        {
			SetSourcFile(filename);
            ReadHeader();

        }

		~DbaseIndexer() {
			DisposeFiles();
		}

		protected void OpenBinaryReader() {
			DisposeFiles();
			_fs = new FileStream(_Filename, FileMode.Open, FileAccess.Read);
			_br = new BinaryReader(_fs);
		}

		private void DisposeFiles()
		{
			if(_br != null)
				_br.Dispose();
			
			if(_fs != null)
				_fs.Dispose();
		}
		
		private void SetSourcFile(string srcFileName) {
		    srcFileName = srcFileName.Remove(srcFileName.Length - 4, 4);
            srcFileName += ".dbf";
            _Filename = srcFileName;
		}

        private void ReadHeader()
        {
			OpenBinaryReader();
        
            byte checkByte;
            byte bbuffer;

            _DbType = _br.ReadByte();

            if (_DbType == 0x03)
            {
                _UpdateYear = _br.ReadByte();
                _UpdateMonth = _br.ReadByte();
                _UpdateDay = _br.ReadByte();
                _RecordCount = _br.ReadUInt32();
                _HeaderLength = _br.ReadUInt16();
                _RecordLength = _br.ReadUInt16();

                // Header size minus terminator char divided by length of
                // a field descriptor minus 1 to adjust for the file descriptor.
                _FieldCount = (uint)((_HeaderLength - 1) / 32) - 1;

                _FieldNames = new string[_FieldCount];
                _FieldTypes = new DbFieldType[_FieldCount];
                _FieldLengths = new byte[_FieldCount];

                for (int a = 0; a < _FieldCount; ++a)
                {
                    _fs.Seek(32 + (32 * a), 0);

                    // Has to be read as byte and converted to char
                    // because a 0x9e is appended to the end of some
                    // field descriptors and it throws off br.ReadChars( x )
                    for (int b = 0; b < 11; ++b)
                    {
                        bbuffer = _br.ReadByte();
                        if (bbuffer == 0) { continue; } // ignore nulls
                        
                        _FieldNames[a] += (char)bbuffer;
                    }

                    _FieldTypes[a] = (DbFieldType)_br.ReadByte();
                    _br.ReadBytes(4); // field address in memory...??
                    _FieldLengths[a] = _br.ReadByte();
                }
            }

            // Field descriptors are 32 bytes each + 32 bytes for the file 
            // descriptor + 1 byte for the header record terminator (0x0D)
            _RecordStart = (_FieldCount * 32) + 32 + 1;

            _fs.Seek(_RecordStart - 1, 0);

            checkByte = _br.ReadByte(); // should be 0x0D (13)

            _br.Close();
        }


        /// <summary>
        /// Gets the names of the fields in the database.
        /// </summary>
        /// @returns A string array containing the field names
        public string[] FieldNames
        {
            get
            {
                return _FieldNames;
            }
        }
    }
}
