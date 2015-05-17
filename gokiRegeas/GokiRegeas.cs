using GokiLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace gokiRegeas
{
    internal class GokiRegeas
    {
        internal static readonly string settingsPath = @".\settings.json";
        internal static readonly int[] lengths = new int[] { 10000, 25000, 30000, 45000, 60000, 75000, 90000 };
        internal static readonly int[] updateIntervals = new int[] { 15, 30, 40, 100, 500 };

        internal static Settings settings;
        internal static double length;
        internal static TimeSpan runningTime;
        internal static Random random;
        internal static DateTime startTime;
        internal static DateTime lastTime;
        internal static DateTime pauseTime;
        internal static TimeSpan timeRemaining;
        internal static bool paused;
        internal static List<string> filePool;
        internal static string currentFilePath;
        internal static Bitmap currentFileBitmap;
        internal static Regex fileFilter;
        internal static List<string> pathHistory;
        internal static int pathHistoryIndex;
        internal static bool horizontalFlip;
        internal static bool verticalFlip;
        internal static double viewRotation;
        internal static double percentage;
        internal static double lastUsedTime;
        internal static Process process;

        
        static GokiRegeas()
        {
            settings = new Settings();
            process = Process.GetCurrentProcess();
            random = new Random();
            startTime = DateTime.Now;
            pauseTime = DateTime.Now;
            length = lengths[2];
            runningTime = TimeSpan.FromMilliseconds(length);
            timeRemaining = TimeSpan.FromMilliseconds(0);
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
            percentage = 0;
            lastUsedTime = 30000;
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
            File.WriteAllText(settingsPath, JsonConvert.SerializeObject(settings,Formatting.Indented));
        }

        internal static void loadSettings()
        {
            try
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(settingsPath));
            }
            catch ( Exception ex)
            {
                settings = new Settings();
                Console.WriteLine("Could not load settings.");
            }
        }

        internal static void fillFilePool()
        {
            filePool.Clear();

            foreach (string path in settings.SessionPaths)
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
                                    if (!filePool.Contains(file))
                                    {
                                        filePool.Add(file);
                                    }
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
