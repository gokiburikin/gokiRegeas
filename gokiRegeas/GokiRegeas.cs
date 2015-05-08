﻿using GokiLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        internal static TimeSpan runningTime;
        internal static Random random;
        internal static DateTime startTime;
        internal static DateTime lastTime;
        internal static DateTime pauseTime;
        internal static TimeSpan timeRemaining;
        internal static bool paused;
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
        internal static bool horizontalFlip;
        internal static bool verticalFlip;
        internal static double viewRotation;
        internal static bool showBigTimer;
        internal static bool alwaysShowTimer;
        internal static double percentage;
        internal static double lastUsedTime;
        internal static Boolean alwaysOnTop;
        internal static int timerOpacity;
        internal static Process process;

        static GokiRegeas()
        {
            process = Process.GetCurrentProcess();
            random = new Random();
            startTime = DateTime.Now;
            pauseTime = DateTime.Now;
            length = lengths[2];
            runningTime = TimeSpan.FromMilliseconds(length);
            timeRemaining = TimeSpan.FromMilliseconds(0);
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
            showBigTimer = false;
            alwaysShowTimer = true;
            percentage = 0;
            lastUsedTime = 30000;
            alwaysOnTop = false;
            timerOpacity = 10;
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
            writer.write(showBigTimer);
            writer.write(alwaysShowTimer);
            writer.write(lastUsedTime);
            writer.write(alwaysOnTop);
            writer.write(timerOpacity);
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
                showBigTimer = reader.readBoolean();
                try
                {
                    alwaysShowTimer = reader.readBoolean();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not load always show timer value.");
                }
                try
                {
                    lastUsedTime = reader.readDouble();
                    length = lastUsedTime;
                    runningTime = TimeSpan.FromMilliseconds(length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not load last used time.");
                }
                try
                {
                    alwaysOnTop = reader.readBoolean();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not load last used time.");
                }
                try
                {
                    timerOpacity= reader.readInt();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not load timer opacity.");
                }
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
