using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Json;
using MKCoolsoft.GPXLib;


namespace starlineonline2gpx
{
    class Date
    {
        private static DateTime UnixEpoch =
        new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static TimeZoneInfo TimeZone = TimeZoneInfo.Local;

        public static long minSeconds = long.MaxValue;
        public static long maxSeconds = long.MinValue;

        public static DateTime DateTimeFromUnixTimestampSeconds(long seconds)
        {
            if (seconds < minSeconds) minSeconds = seconds;
            if (seconds > maxSeconds) maxSeconds = seconds;
            return UnixEpoch.AddSeconds(seconds);
        }
    }

    class Program
    {
        static decimal Minlat = decimal.MaxValue;
        static decimal Maxlat = decimal.MinValue;
        static decimal Minlon = decimal.MaxValue;
        static decimal Maxlon = decimal.MinValue;

        static void Main(string[] args)
        {
            if (args.Length > 0 && File.Exists(args[0]))
            {
                var path = args[0];
                using (StreamReader sr = new StreamReader(path))
                {
                    var json = JsonValue.Parse(sr.ReadToEnd());

                    GPXLib gpx = new GPXLib();

                    var meta = json["meta"];
                    var data = json["data"];
                    var trackNum = 0;
                    var waypointNum = 0;
                    foreach(JsonValue elem in data)
                    {
                        string type = elem["type"];
                        switch(type)
                        {
                            case "TZ":
                                ParseTimezone(gpx, elem);
                                break;
                            case "TRACK":
                                ParseTrack(gpx, elem, ++trackNum);
                                break;
                            case "NO_SIGNAL":
                                ParseNoSignal(gpx, elem);
                                break;
                            case "WAYPOINT":
                                ParseWaypoint(gpx, elem, ++waypointNum);
                                break;
                            case "STOP":
                                ParseStop(gpx, elem);
                                break;
                            default:
                                throw new NotImplementedException();

                        }
                    }


                    gpx.Metadata.Name = "Starline GPX Track";

                    var startDate = Date.DateTimeFromUnixTimestampSeconds(Date.minSeconds);
                    var endDate = Date.DateTimeFromUnixTimestampSeconds(Date.maxSeconds);
                    gpx.Metadata.Desc = String.Format("Track from {0} to {1}", startDate, endDate);

                    gpx.Creator = "starlineonline2gpx converter";

                    gpx.Metadata.Bounds.Maxlat = Maxlat;
                    gpx.Metadata.Bounds.Minlat = Minlat;
                    gpx.Metadata.Bounds.Maxlon = Maxlon;
                    gpx.Metadata.Bounds.Minlon = Minlon;
                    gpx.Metadata.Time = startDate;
                    gpx.Metadata.TimeSpecified = true;

                    var author = new Person();
                    author.Link.Href = @"https://github.com/DJm00n/starlineonline2gpx";
                    author.Email.Id = @"dimitriy.ryazantcev";
                    author.Email.Domain = "gmail.com";
                    author.Name = @"Dimitriy Ryazantcev";

                    gpx.Metadata.Author = author;


                    gpx.SaveToFile(Path.ChangeExtension(path, "gpx"));
                }
            }
        }

        private static void TrackMinMax(decimal lat, decimal lon)
        {
            if (lat < Minlat) Minlat = lat;
            if (lat > Maxlat) Maxlat = lat;

            if (lon < Minlon) Minlon = lon;
            if (lon > Maxlon) Maxlon = lon;
        }

        private static void ParseNoSignal(GPXLib gpx, JsonValue elem)
        {
            //throw new NotImplementedException();
        }

        private static void ParseStop(GPXLib gpx, JsonValue elem)
        {
            //throw new NotImplementedException();
        }

        private static void ParseWaypoint(GPXLib gpx, JsonValue elem, int waypointNum)
        {
            Wpt waypoint = new Wpt
            {
                Name = String.Format("Waypoint {0}", waypointNum),
                Lat = elem["x"],
                Lon = elem["y"],
                Ele = elem["z"],
                EleSpecified = elem.ContainsKey("z") && elem["z"] != 0,
                Sat = Convert.ToString((int)elem["sat_qty"]),
                Time = Date.DateTimeFromUnixTimestampSeconds(elem["t"]),
                TimeSpecified = elem.ContainsKey("t")
            };

            gpx.WptList.Add(waypoint);
            TrackMinMax(waypoint.Lat, waypoint.Lon);
        }

        private static void ParseTimezone(GPXLib gpx, JsonValue elem)
        {
            string displayName = "Local Time Zone";
            string standardName = "Local Time";
            TimeSpan offset = TimeSpan.FromSeconds(elem["time_shift"]);
            TimeZoneInfo local = TimeZoneInfo.CreateCustomTimeZone(standardName, offset, displayName, standardName);
            Date.TimeZone = local;
            //TODO save this timezone somewhere in gpx file
        }

        private static void ParseTrack(GPXLib gpx, JsonValue elem, int trackNum)
        {
            Trkseg segment = new Trkseg();

            foreach (JsonValue node in elem["nodes"])
            {
                Wpt wpt = new Wpt
                {
                    Lat = node["x"],
                    Lon = node["y"],
                    Ele = node["z"],
                    EleSpecified = node.ContainsKey("z") && node["z"] != 0,
                    Sat = Convert.ToString((int)node["sat_qty"]),
                    Time = Date.DateTimeFromUnixTimestampSeconds(node["t"]),
                    TimeSpecified = node.ContainsKey("t")
                };

                segment.TrkptList.Add(wpt);
                TrackMinMax(wpt.Lat, wpt.Lon);
            }

            Trk track = new Trk(String.Format("Track {0}", trackNum))
            {
                Number = trackNum.ToString()
            };
            track.TrksegList.Add(segment);

            gpx.TrkList.Add(track);
        }
    }
}
