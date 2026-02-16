using System;
using System.Globalization;
using System.Text;

namespace NavigationSimulator.Helpers
{
    public static class NmeaHelpers
    {
        public static string CreateGga(DateTime time, double lat, double lon, double alt)
        {
            string timeStr = time.ToString("HHmmss.fff", CultureInfo.InvariantCulture);
            
            double latAbs = Math.Abs(lat);
            int latDeg = (int)latAbs;
            double latMin = (latAbs - latDeg) * 60.0;
            string latDir = lat >= 0 ? "N" : "S";
            string latStr = $"{latDeg:D2}{latMin:00.0000}";

            double lonAbs = Math.Abs(lon);
            int lonDeg = (int)lonAbs;
            double lonMin = (lonAbs - lonDeg) * 60.0;
            string lonDir = lon >= 0 ? "E" : "W";
            string lonStr = $"{lonDeg:D3}{lonMin:00.0000}";

            // $GPGGA,hhmmss.ss,llll.ll,a,yyyyy.yy,a,x,xx,x.x,x.x,M,x.x,M,x.x,xxxx*hh
            string sentence = $"GPGGA,{timeStr},{latStr},{latDir},{lonStr},{lonDir},1,08,1.0,{alt:F1},M,0.0,M,,";
            return AppendChecksum(sentence);
        }

        public static string CreateVtg(double courseTrueRad, double speedMs)
        {
            double courseDeg = courseTrueRad * 180.0 / Math.PI;
            while (courseDeg < 0) courseDeg += 360;
            while (courseDeg >= 360) courseDeg -= 360;

            double speedKnots = speedMs * 1.94384;
            double speedKph = speedMs * 3.6;

            // $GPVTG,xxx.x,T,xxx.x,M,xxx.x,N,xxx.x,K,a*hh
            string sentence = $"GPVTG,{courseDeg:F1},T,,M,{speedKnots:F1},N,{speedKph:F1},K,A";
            return AppendChecksum(sentence);
        }

        private static string AppendChecksum(string sentence)
        {
            byte checksum = 0;
            foreach (char c in sentence)
            {
                checksum ^= (byte)c;
            }
            return $"${sentence}*{checksum:X2}";
        }
    }
}
