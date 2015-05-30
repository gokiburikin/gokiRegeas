using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace gokiRegeas
{
    public class Settings
    {
        private List<string> paths;

        public List<string> Paths
        {
            get { return paths; }
            set { paths = value; }
        }
        private List<string> sessionPaths;

        public List<string> SessionPaths
        {
            get { return sessionPaths; }
            set { sessionPaths = value; }
        }
        private Color backColor;

        public Color BackColor
        {
            get { return backColor; }
            set { backColor = value; }
        }
        private bool showBigTimer;

        public bool ShowBigTimer
        {
            get { return showBigTimer; }
            set { showBigTimer = value; }
        }
        private bool alwaysShowTimer;

        public bool AlwaysShowTimer
        {
            get { return alwaysShowTimer; }
            set { alwaysShowTimer = value; }
        }
        private Boolean alwaysOnTop;

        public Boolean AlwaysOnTop
        {
            get { return alwaysOnTop; }
            set { alwaysOnTop = value; }
        }
        private int timerOpacity;

        public int TimerOpacity
        {
            get { return timerOpacity; }
            set { timerOpacity = value; }
        }
        private bool convertToGreyscale;

        public bool ConvertToGreyscale
        {
            get { return convertToGreyscale; }
            set { convertToGreyscale = value; }
        }
        private bool resetViewOnImageChange;

        public bool ResetViewOnImageChange
        {
            get { return resetViewOnImageChange; }
            set { resetViewOnImageChange = value; }
        }
        private int updateInterval;

        public int UpdateInterval
        {
            get { return updateInterval; }
            set { updateInterval = value; }
        }

        private int qualityAdjustDelay;

        public int QualityAdjustDelay
        {
            get { return qualityAdjustDelay; }
            set { qualityAdjustDelay = value; }
        }

        public Settings()
        {
            Paths = new List<string>();
            SessionPaths = new List<string>();
            BackColor = Color.White;
            ConvertToGreyscale = false;
            ResetViewOnImageChange = true;
            ShowBigTimer = false;
            AlwaysShowTimer = true;
            TimerOpacity = 127;
            UpdateInterval = 40;
            AlwaysOnTop = false;
            QualityAdjustDelay = 1000;
        }
    }
}
