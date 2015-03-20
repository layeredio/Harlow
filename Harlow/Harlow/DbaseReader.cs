using System;
using System.IO;

namespace Harlow
{
    /// <summary>
    /// The class that provides access to the .dbf database file.
    /// </summary>
    public class DbaseReader : DbaseIndexer
    {

        public DbaseReader(string filename)
            : base(filename)
        {
        }


        /// <summary>
        /// Get a string array that represents a record, one string for
        /// each field in the record.
        /// </summary>
        /// @return A string array that represents one record.
        public string[] this[int index]
        {
            get
            {
                return GetRecord(index);
            }
        }

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
            FileStream fs = new FileStream(_Filename, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);

            string[] buffer = new string[_FieldCount];

            if (index < _RecordCount)
            {
                fs.Seek(_RecordStart + (index * _RecordLength), 0);
            }
            else
            {
                return null;
            }

            br.ReadByte();  // delete flag

            for (int a = 0; a < _FieldCount; ++a)
            {
                char[] chars;

                switch (_FieldTypes[a])
                {
                    case FieldType.C:
                    case FieldType.c:
                        //buffer[ a ] = new string( br.ReadChars( _FieldLengths[ a ] ) );
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;

                    case FieldType.D:
                    case FieldType.d:
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;

                    case FieldType.F:
                    case FieldType.f:
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;

                    case FieldType.L:
                    case FieldType.l:
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;

                    case FieldType.N:
                    case FieldType.n:
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;

                    default:
                        chars = new char[_FieldLengths[a]];
                        for (int i = 0; i < _FieldLengths[a]; i++)
                        {
                            chars[i] = (char)br.ReadByte();
                        }
                        buffer[a] = new string(chars);
                        break;
                }
            }

            br.Close();

            return buffer;
        }
    }
}
