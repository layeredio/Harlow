using System;
using System.IO;
using Newtonsoft.Json;

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

    public class ShapeFileReader : ShapefileIndexer
    {
        
        private VectorFeature[] _Features;
        private DbaseReader _Dbase;

        public ShapeFileReader(string filename) : base(filename)
        {
            _Dbase = new DbaseReader(filename);
        }

        /// <summary>
        /// Native feature array as read from the Shapefile.
        /// </summary>
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
        /// Native feature array converted to an array of GeoJson objects.
        /// </summary>
        /// <returns>A GeoJson array of all features.</returns>
        public string FeaturesAsJson()
        {
            if (_Features == null)
                LoadFile();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonResolver();

            return JsonConvert.SerializeObject(_Features, settings);
        }

        /// <summary>
        /// Get a specific feature as Json.
        /// </summary>
        /// <param name="ordinal"></param>
        /// <returns>A single GeoJson feature.</returns>
        public string FeatureAsJson(int ordinal)
        {
            if (_Features == null)
                LoadFile();

            var settings = new JsonSerializerSettings();
            settings.ContractResolver = new JsonResolver();

            return JsonConvert.SerializeObject(_Features[ordinal], settings);
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
                    tempFeature = new VectorFeature(1, _ShapeType);
                    tempPoints = new PointD[1];

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = Enum.GetName(typeof(ShapeType), br.ReadInt32());
                    tempPoints[0] = new PointD(br.ReadDouble(), br.ReadDouble());

                    // So geoindexing works correctly
                    tempFeature.Bbox[0] = tempPoints[0].Value[0]; // X
                    tempFeature.Bbox[1] = tempPoints[0].Value[1]; // Y

                    tempFeature.Coordinates.Add(tempPoints);

                    int colNum = 0;
                    foreach(string col in _Dbase.FieldNames)
                    {
                        tempFeature.Properties.Add(col.Trim().ToLower(), _Dbase[a][colNum].Trim().ToLower());
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
                    tempFeature = new VectorFeature(segmentCount, _ShapeType);

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = Enum.GetName(typeof(ShapeType), br.ReadInt32());
                    tempFeature.Bbox[0] = br.ReadDouble(); // X
                    tempFeature.Bbox[1] = br.ReadDouble(); // Y
                    tempFeature.Bbox[2] = br.ReadDouble(); // X
                    tempFeature.Bbox[3] = br.ReadDouble(); // Y
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
                        tempFeature.Properties.Add(col.Trim().ToLower(), _Dbase[a][colNum].Trim().ToLower());
                        colNum++;
                    }

                    _Features[a] = tempFeature;
                }
            }

            br.Close();
        }
    }
}
