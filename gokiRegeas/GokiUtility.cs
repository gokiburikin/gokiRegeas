using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GokiLibrary
{
    public class GokiUtility
    {
        private static long tick = 0;
        private static long updateInterval = 20;
        private static long lastTime = TimeSinceEpoch;
        private static Stopwatch performanceStopwatch;
        private static bool acceptingScreenshots = true;

       
        static GokiUtility()
        {
            performanceStopwatch = new Stopwatch();
        }

        public static void AcceptScreenshots()
        {
            acceptingScreenshots = true;
        }


        public static void setInterval(long value)
        {
            updateInterval = value;
        }

        public static bool IntervalPassed
        {
            get
            {
                bool passed = false;
                if (TimeSinceEpoch - lastTime >= updateInterval)
                {
                    passed = true;
                    lastTime = TimeSinceEpoch;
                }
                return passed;
            }
        }

        public static long TimeSinceEpoch
        {
            get
            {
                return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
            }
        }

        public static void startTrackingPerformance()
        {
            performanceStopwatch.Restart();
        }

        public static long stopTrackingPerformance(bool ticks = true)
        {
            long time = 0;
            if (ticks)
            {
                time = performanceStopwatch.ElapsedTicks;
            }
            else
            {
                time = performanceStopwatch.ElapsedMilliseconds;
            }
            performanceStopwatch.Stop();
            return time;
        }

        public static string getCompressedString(string input)
        {
            using (MemoryStream inputStream = new MemoryStream(System.Text.Encoding.Unicode.GetBytes(input)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(outputStream, CompressionMode.Compress))
                    {
                        inputStream.CopyTo(zipStream);
                    }
                    return byteArrayToString(outputStream.ToArray());
                }
            }
        }

        public static string getDecompressedString(string input)
        {
            using (MemoryStream inputStream = new MemoryStream(stringToByteArray(input)))
            {
                using (MemoryStream outputStream = new MemoryStream())
                {
                    using (GZipStream zipStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        zipStream.CopyTo(outputStream);
                    }
                    return System.Text.Encoding.Unicode.GetString(outputStream.ToArray());
                }
            }
        }

        public static byte[] stringToByteArray(string input)
        {
            if ( input == null )
            {
                return new byte[0];
            }
            byte[] output = new byte[input.Length * sizeof(char)];
            System.Buffer.BlockCopy(input.ToCharArray(), 0, output, 0, output.Length);
            return output;
        }

        public static string byteArrayToString(byte[] input)
        {
            char[] chars = new char[input.Length];
            System.Buffer.BlockCopy(input, 0, chars, 0, chars.Length);
            string output = new string(chars).Substring(0, input.Length >> 1);
            return output;
        }

        public static void writeCompressedByteArray(byte[] input, string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                GZipStream gzip = new GZipStream(fileStream, CompressionMode.Compress);
                gzip.Write(input, 0, input.Length);
                gzip.Close();
            }
        }

        public static byte[] readCompressedByteArray(string filePath)
        {
            byte[] output;
            using (GZipStream stream = new GZipStream(new MemoryStream(File.ReadAllBytes(filePath)), CompressionMode.Decompress))
            {
                const int size = 4096;
                byte[] buffer = new byte[size];
                using (MemoryStream memory = new MemoryStream())
                {
                    int count = 0;
                    do
                    {
                        count = stream.Read(buffer, 0, size);
                        if (count > 0)
                        {
                            memory.Write(buffer, 0, count);
                        }
                    }
                    while (count > 0);
                    output = memory.ToArray();
                }
            }
            return output;
        }

        public static byte[] getCompressedByteArray(byte[] input, int capacity = 2)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    using (MemoryStream inputStream = new MemoryStream(input))
                    {
                        inputStream.CopyTo(gzip);
                    }
                }
                return memoryStream.ToArray();
            }
        }

        public static byte[] getDecompressedByteArray(byte[] input)
        {
            using (MemoryStream inputStream = new MemoryStream(input))
            {
                using (GZipStream stream = new GZipStream(inputStream, CompressionMode.Decompress))
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        stream.CopyTo(outputStream);
                        return outputStream.ToArray();
                    }
                }
            }
        }

        public static Rectangle getRectangleContracted(Rectangle original, int amount)
        {
            return new Rectangle(original.X + amount, original.Y + amount, original.Width - amount*2, original.Height - amount*2);
        }

        public static Rectangle getRectangleContracted(Rectangle original, int left, int top, int right, int bottom)
        {
            return new Rectangle(original.X + left, original.Y + top, original.Width - right - left, original.Height - bottom - top);
        }

        // From Eric Woroshow @ ericw.ca
        private static IEnumerable<Point> getUnsortedPointsOnLine(int x0, int y0, int x1, int y1)
        {
            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Point((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        public static List<Point> getPointsOnLine( int x1, int y1, int x2, int y2 )
        {
            List<Point> points = getUnsortedPointsOnLine(x1, y1, x2, y2).ToList();
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if ((x1 > x2 && (y2 < y1 || !steep)) || x1 < x2 && (y2 < y1 && steep))
            {
                for (int i = 0; i < points.Count; i++)
                {
                    points[i] = new Point(x2 - points[i].X + x1, y2 - points[i].Y + y1);
                }
            }
            return points;
        }

        public static double Tick
        {
            get
            {
                return tick++;
            }
        }

        public static Point FromPointF(PointF pointf)
        {
            return new Point((int)pointf.X, (int)pointf.Y);
        }
    }
}
