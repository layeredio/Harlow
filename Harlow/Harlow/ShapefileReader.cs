using System;
using System.IO;

namespace Harlow
{

    public enum ShapeType
    {
        Null = 0,
        Point = 1,
        MultiLineString = 3,
        MultiPolygon = 5,
        Multipoint = 8
    };


    public class ShapeFileReader
    {
        private int _Version;
        private ShapeType _ShapeType;
        private string _Filename;
        private int _IndexFileSize;
        private double[] _BBox;
        private int[] _OffsetOfRecord;
        private int[] _LengthOfRecord;
        private int _FeatureCount;
        private VectorFeature[] _Features;
        private Dbase _Dbase;


        public ShapeFileReader(string filename)
        {
            _Filename = filename;
            _BBox = new double[4];
            _Dbase = new Dbase(filename);

            ParseHeader(filename);// .shx file
            ReadIndex(filename); // .shx file
        }

        public VectorFeature[] Features
        {
            get
            {
                if (_Features == null)
                    LoadFile();

                return _Features; 
            }
        }

        /// <summary>
        /// Loads the VectorFeature array with the features from the
        /// shapefile.
        /// </summary>
        public void LoadFile()
        {
            FileStream fs = new FileStream(_Filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            VectorFeature tempFeature;
            int[] segments;
            int segmentPosition;
            int pointsInSegment;
            PointD[] tempPoints;
            PointD[] segmentPoints;

            _Features = new VectorFeature[_FeatureCount];


            if (_ShapeType == ShapeType.Point)
            {
                for (int a = 0; a < _FeatureCount; ++a)
                {
                    // Point types don't have parts (segments) / one point per feature
                    tempFeature = new VectorFeature(1);
                    tempPoints = new PointD[1];

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = (ShapeType)br.ReadInt32();
                    tempPoints[0] = new PointD(br.ReadDouble(), br.ReadDouble());

                    // So geoindexing works correctly
                    tempFeature.Bbox[0] = tempPoints[0].X;
                    tempFeature.Bbox[1] = tempPoints[0].Y;
                    tempFeature.Bbox[2] = tempPoints[0].X;
                    tempFeature.Bbox[3] = tempPoints[0].Y;

                    tempFeature.Coordinates.Add(tempPoints);

                    int colNum = 0;
                    foreach(string col in _Dbase.FieldNames)
                    {
                        tempFeature.Properties.Add(col.Trim(), _Dbase[a][colNum].Trim());
                        colNum++;
                    }

                    _Features[a] = tempFeature;
                }
            }
            else
            {
                for (int a = 0; a < _FeatureCount; ++a)
                {
                    fs.Seek(_OffsetOfRecord[a] + 44, 0);
                    int segmentCount = br.ReadInt32();

                    // Read the number of parts (segments) and create a new VectorFeature
                    tempFeature = new VectorFeature(segmentCount);

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = (ShapeType)br.ReadInt32();
                    tempFeature.Bbox[0] = br.ReadDouble();
                    tempFeature.Bbox[1] = br.ReadDouble();
                    tempFeature.Bbox[2] = br.ReadDouble();
                    tempFeature.Bbox[3] = br.ReadDouble();
                    br.ReadInt32(); // Number of parts (segments) gotten earlier
                    tempPoints = new PointD[br.ReadInt32()]; // Number of points

                    segments = new int[segmentCount + 1];

                    //Read in the segment indexes
                    for (int b = 0; b < segmentCount; ++b)
                    {
                        segments[b] = br.ReadInt32();
                    }

                    //Read in *ALL* of the points in the feature
                    for (int c = 0; c < tempPoints.Length; ++c)
                    {
                        tempPoints[c] = new PointD(br.ReadDouble(), br.ReadDouble());
                    }

                    //Add in an ending point for the inner loop that follows (e) 
                    segments[segmentCount] = tempPoints.Length;

                    //Watch your step...
                    for (int d = 0; d < segmentCount; ++d)
                    {
                        pointsInSegment = segments[d + 1] - segments[d];
                        segmentPoints = new PointD[pointsInSegment];
                        segmentPosition = 0;

                        for (int e = segments[d]; e < segments[d + 1]; ++e)
                        {
                            segmentPoints[segmentPosition] = tempPoints[e];
                            ++segmentPosition;
                        }

                        tempFeature.Coordinates.Add(segmentPoints);
                    }

                    int colNum = 0;
                    foreach (string col in _Dbase.FieldNames)
                    {
                        tempFeature.Properties.Add(col.Trim(), _Dbase[a][colNum].Trim());
                        colNum++;
                    }

                    _Features[a] = tempFeature;
                }
            }

            br.Close();
        }



        /// <summary>
        /// Reads the header of the .shx index file and extracts the 
        /// information that is needed to read the remainder of the
        /// .shx file and the .shp file.
        /// </summary>
        /// @param filename The name of the Shapefile. (ie. "blah.shp")
        private void ParseHeader(string filename)
        {
            filename = filename.Remove(filename.Length - 4, 4);
            filename += ".shx";

            FileStream fs = new FileStream(filename, FileMode.Open);
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
                header_b[i] = br.ReadDouble();
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
        /// @param filename The name of the Shapefile. (ie. "blah.shp")
        private void ReadIndex(string filename)
        {
            filename = filename.Remove(filename.Length - 4, 4);
            filename += ".shx";

            FileStream fs = new FileStream(filename, FileMode.Open);
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
        ///@param i The int32 integer to byte swap
        protected int SwitchByteOrder(int i)
        {
            byte[] buffer = new byte[4];
            buffer = BitConverter.GetBytes(i);
            Array.Reverse(buffer, 0, buffer.Length);

            return BitConverter.ToInt32(buffer, 0);
        }
    }
}
