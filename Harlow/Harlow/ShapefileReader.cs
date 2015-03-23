using System;
using System.IO;
using Newtonsoft.Json;

namespace Harlow
{
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

            var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
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

            var settings = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
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
            //VectorFeature tempFeature;
            int[] segments;
            int segmentPosition;
            int pointsInSegment;
            PointA[] segmentPoints;

            if (_ShapeType == ShapeType.Point)
            {
                _Features = new VectorPoint[_FeatureCount];

                for (int a = 0; a < _FeatureCount; ++a)
                {
                    // Point types don't have parts (segments) / one point per feature
                    VectorPoint tempFeature = new VectorPoint(1, _ShapeType);
                    tempFeature.Coordinates = new double[2];

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = Enum.GetName(typeof(ShapeType), br.ReadInt32());
                    tempFeature.Coordinates[0] = br.ReadDouble();
                    tempFeature.Coordinates[1] = br.ReadDouble();

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
                _Features = new VectorShape[_FeatureCount];
                PointA[] tempPoints;

                for (int a = 0; a < _FeatureCount; ++a)
                {
                    fs.Seek(_OffsetOfRecord[a] + 44, 0);
                    int segmentCount = br.ReadInt32();

                    // Read the number of parts (segments) and create a new VectorFeature
                    VectorShape tempFeature = new VectorShape(segmentCount, _ShapeType);

                    fs.Seek(_OffsetOfRecord[a], 0);

                    br.ReadInt32(); //Record number (not needed)
                    br.ReadInt32(); //Content length (not needed)
                    tempFeature.Type = Enum.GetName(typeof(ShapeType), br.ReadInt32());
                    tempFeature.Bbox[0] = br.ReadDouble(); // X
                    tempFeature.Bbox[1] = br.ReadDouble(); // Y
                    tempFeature.Bbox[2] = br.ReadDouble(); // X
                    tempFeature.Bbox[3] = br.ReadDouble(); // Y
                    br.ReadInt32(); // Number of parts (segments) gotten earlier
                    tempPoints = new PointA[br.ReadInt32()]; // Number of points

                    segments = new int[segmentCount + 1];

                    //Read in the segment indexes
                    for (int b = 0; b < segmentCount; ++b)
                    {
                        segments[b] = br.ReadInt32();
                    }

                    //Read in *ALL* of the points in the feature
                    for (int c = 0; c < tempPoints.Length; ++c)
                    {
                        tempPoints[c] = new PointA(br.ReadDouble(), br.ReadDouble());
                    }

                    //Add in an ending point for the inner loop that follows (e) 
                    segments[segmentCount] = tempPoints.Length;

                    //Watch your step...
                    for (int d = 0; d < segmentCount; ++d)
                    {
                        pointsInSegment = segments[d + 1] - segments[d];
                        segmentPoints = new PointA[pointsInSegment];
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
