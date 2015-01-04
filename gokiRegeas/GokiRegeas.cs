using GokiLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace gokiRegeas
{
    internal class GokiRegeas
    {
        internal static readonly string settingsPath = @".\settings.dat";
        internal static readonly int[] lengths = new int[] { 10000, 25000, 30000, 45000, 60000, 75000, 90000 };

        internal static double length;
        internal static Random random;
        internal static DateTime startTime;
        internal static List<string> paths;
        internal static List<string> sessionPaths;
        internal static List<string> filePool;
        internal static string currentFilePath;
        internal static Bitmap currentFileBitmap;
        internal static Bitmap resizedCurrentFileBitmap;
        internal static Regex fileFilter;
        internal static List<string> pathHistory;
        internal static int pathHistoryIndex;
        internal static Color backColor;
        internal static bool paused;
        internal static DateTime pauseTime;
        internal static bool horizontalFlip;
        internal static bool verticalFlip;
        internal static double viewRotation;

        static GokiRegeas()
        {
            random = new Random();
            startTime = DateTime.Now;
            pauseTime = DateTime.Now;
            length = lengths[2];
            paths = new List<string>();
            sessionPaths = new List<string>();
            paths.Add(@".\img\");
            backColor = Color.DarkSlateGray;
            currentFilePath = "";
            currentFileBitmap = null;
            fileFilter = new Regex(@".*(\.jpg|\.jpeg|\.png|\.gif|\.bmp)");
            filePool = new List<string>();
            pathHistory = new List<string>();
            pathHistoryIndex = 0;
            paused = false;
            horizontalFlip = false;
            verticalFlip = false;
            viewRotation = 0;
        }

        internal static void pause()
        {
            pauseTime = DateTime.Now;
            paused = true;
        }

        internal static void unpause()
        {
            startTime = startTime.Add(DateTime.Now - pauseTime);
            paused = false;
        }

        internal static void togglePause()
        {
            if (paused)
            {
                unpause();
            }
            else
            {
                pause();
            }
        }

        internal static void saveSettings()
        {
            AutoSizeGokiBytesWriter writer = new AutoSizeGokiBytesWriter();
            writer.write(paths.Count);
            foreach (string path in paths)
            {
                writer.write(path);
            }
            writer.write(sessionPaths.Count);
            foreach (string path in sessionPaths)
            {
                writer.write(path);
            }
            writer.write(backColor);
            File.WriteAllBytes(settingsPath, writer.Data);
        }

        internal static void loadSettings()
        {
            try
            {
                if (!File.Exists(settingsPath))
                {
                    saveSettings();
                }
                paths.Clear();
                sessionPaths.Clear();
                GokiBytesReader reader = new GokiBytesReader(File.ReadAllBytes(settingsPath));

                int pathCount = reader.readInt();
                for (int i = 0; i < pathCount; i++)
                {
                    paths.Add(reader.readString());
                }
                int sessionPathCount = reader.readInt();
                for (int i = 0; i < sessionPathCount; i++)
                {
                    string path = reader.readString();
                    if (paths.Contains(path))
                    {
                        sessionPaths.Add(path);
                    }
                }
                backColor = reader.readColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not load settings.");
            }
        }

        internal static void fillFilePool()
        {
            filePool.Clear();

            foreach (string path in sessionPaths)
            {
                try
                {
                    if (Directory.Exists(path))
                    {
                        List<string> files = new List<string>();
                        files.AddRange(Directory.EnumerateFiles(path, "*", SearchOption.TopDirectoryOnly));
                        files.AddRange(Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories));
                        for (int i = 0; i < files.Count; i++)
                        {
                            string file = files[i];
                            if (Directory.Exists(file))
                            {
                                files.AddRange(Directory.EnumerateFiles(file, "*", SearchOption.TopDirectoryOnly));
                                files.AddRange(Directory.EnumerateDirectories(file, "*", SearchOption.TopDirectoryOnly));
                            }
                            else
                            {
                                if (GokiRegeas.fileFilter.IsMatch(file.ToLower()))
                                {
                                    filePool.Add(file);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
