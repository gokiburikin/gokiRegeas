using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GokiLibrary
{
    public class ColorChangedEventArgs
    {
        private Color color;

        public Color Color
        {
            get { return color; }
            set { color = value; }
        }
        public ColorChangedEventArgs(Color color)
        {
            Color = color;
        }
    }
    public class GokiColor
    {
        #region Predefined Colors 
        public static readonly Color Sadosa = Color.FromArgb(255, 90, 208, 90);
        public static readonly Color TegakiRed = Color.FromArgb(255, 127, 0, 0);
        public static readonly Color TegakiBeige = Color.FromArgb(255, 240, 224, 214);
        public static readonly GokiColor Empty = GokiColor.fromARGB(0,0,0,0);
        #endregion Predefined Colors
        
        #region Static Methods

        private static Random colorRandom = new Random();
        public static Color multiplied(Color original, float scalar)
        {
            byte a = (byte)original.A;
            byte r = (byte)Math.Min(Math.Max(original.R * scalar, 0), 255);
            byte g = (byte)Math.Min(Math.Max(original.G * scalar, 0), 255);
            byte b = (byte)Math.Min(Math.Max(original.B * scalar, 0), 255);
            return Color.FromArgb(a, r, g, b);
        }

        public static GokiColor fromColor(System.Drawing.Color color)
        {
            return GokiColor.fromARGB(color.A, color.R, color.G, color.B);
        }

        public static GokiColor fromARGB(int argb)
        {
            byte r = (byte)(argb>>16);
            byte g = (byte)(argb>>8);
            byte b = (byte)(argb);
            byte a = (byte)(argb>>24);
            return GokiColor.fromARGB(a, r, g, b);
        }

        public static Color fromInfo(byte[] info)
        {
            return Color.FromArgb(info[0], info[1], info[2], info[3]);
        }

        public static GokiColor fromARGB(float r, float g, float b)
        {
            GokiColor output = new GokiColor();
            output.Red = r;
            output.Green = g;
            output.Blue = b;
            output.Alpha = 255;
            return output;
        }

        public static GokiColor fromARGB(float a, float r, float g, float b)
        {
            GokiColor output = new GokiColor();
            output.Red = r;
            output.Green = g;
            output.Blue = b;
            output.Alpha = a;
            return output;
        }

        public static GokiColor fromBGRA(byte b, byte g, byte r, byte a)
        {
            return fromARGB(a,r,g,b);
        }

        public static GokiColor fromHSV(float hue, float saturation, float value)
        {
            GokiColor output = new GokiColor();
            output.Hue = hue;
            output.Saturation = saturation;
            output.Value = value;
            return output;
        }

        public static int infoFromARGB(byte a, byte r, byte g, byte b)
        {
            return (a << 24) | (r << 16) | (g << 8) | (b);
        }

        public static int infoFromBGRA(byte b, byte g, byte r, byte a)
        {
            return (a << 24) | (r << 16) | (g << 8) | (b);
        }

        public static byte[] extractInfo(Color color)
        {
            int argb = color.ToArgb();
            return new byte[] { (byte)(argb >> 24), (byte)(argb >> 16 ), (byte)(argb >> 8), (byte)(argb ) };
        }

        public static void extractInfo(ref byte[] data, int argb)
        {
            data[0] = (byte)(argb >> 16);
            data[1] = (byte)(argb >> 8);
            data[2] = (byte)(argb);
            data[3] = (byte)(argb >> 24);
        }

        public static float average(Color color)
        {
            return (color.R + color.B + color.G) / 3.0f;
        }

        public static int sum(Color color)
        {
            return color.R + color.B + color.G;
        }

        public static Color transparent(Color original, byte alpha)
        {
            return Color.FromArgb(alpha, original.R, original.G, original.B);
        }

        public static Color Random
        {
            get
            {
                return Color.FromArgb((byte)colorRandom.Next(256), (byte)colorRandom.Next(256), (byte)colorRandom.Next(256));
            }
        }

        public static Color inverse(Color original)
        {
            byte a = (byte)original.A;
            byte r = (byte)Math.Min(Math.Max(255 - original.R, 0), 255);
            byte g = (byte)Math.Min(Math.Max(255 - original.G, 0), 255);
            byte b = (byte)Math.Min(Math.Max(255 - original.B, 0), 255);
            return Color.FromArgb(a, r, g, b);
        }


        public static bool compare(Color selectedColor, Color color)
        {
            return selectedColor.ToArgb().Equals(color.ToArgb());
        }

        public static Color between(System.Drawing.Color color1, System.Drawing.Color color2, float power)
        {
            if (power == 0)
            {
                return color1;
            }
            byte r = (byte)(color1.R + (color2.R - color1.R) * power);
            byte g = (byte)(color1.G + (color2.G - color1.G) * power);
            byte b = (byte)(color1.B + (color2.B - color1.B) * power);
            return Color.FromArgb(r, g, b);
        }

        public static void RGBtoHSV(float red, float green, float blue, ref float hue, ref float saturation, ref float value)
        {
            Color color = Color.FromArgb((int)red, (int) green, (int)blue);
            int max = (int)Math.Max(red, Math.Max(green, blue));
            int min = (int)Math.Min(red, Math.Min(green, blue));

            if (max != 0)
            {
                hue = color.GetHue();
                saturation = 1f - (1f * min / max);
                value = max / 255f;
            }
        }

        public static void HSVtoRGB(float hue, float saturation, float value, ref float red, ref float green, ref float blue)
        {
            float h = hue / 360;
            int i = (int)(h * 6);
            float f = h * 6 - i;
            float p = value * (1 - saturation);
            float q = value * (1 - f * saturation);
            float t = value * (1 - (1 - f) * saturation);

            red = value;
            green = value;
            blue = value;
            switch (i % 6)
            {
                case 0:
                    green = t;
                    blue = p;
                    break;
                case 1:
                    red = q;
                    blue = p;
                    break;
                case 2:
                    red = p;
                    blue = t;
                    break;
                case 3:
                    red = p;
                    green = q;
                    break;
                case 4:
                    red = t;
                    green = p;
                    break;
                case 5:
                    green = p;
                    blue = q;
                    break;
            }
            red *= 255;
            green *= 255;
            blue *= 255;
        }

        public byte[] Info()
        {
            return new byte[] { (byte)Blue, (byte)Green, (byte)Red, (byte)Alpha };
        }

        public static byte[] getClosestColor(byte red, byte green, byte blue, byte alpha, GokiColorPalette palette)
        {
            int smallestDifference = int.MaxValue;
            byte[] closest = new byte[4];
            byte[] data = palette.Data;
            int length = palette.Length;
            for (int i = 0; i < length; i += 4)
            {
                byte searchBlue = data[i + 0];
                byte searchGreen = data[i + 1];
                byte searchRed = data[i + 2];
                byte searchAlpha = data[i + 3];

                int difference = GokiColor.difference(red, green, blue, alpha, searchRed, searchGreen, searchBlue, searchAlpha);

                if (difference < smallestDifference)
                {
                    smallestDifference = difference;
                    closest[0] = searchBlue;
                    closest[1] = searchGreen;
                    closest[2] = searchRed;
                    closest[3] = searchAlpha;
                }
            }
            return closest;
        }


        public static int difference(byte testRed, byte testGreen, byte testBlue, byte testAlpha, byte comparisonRed, byte comparisonGreen, byte comparisonBlue, byte comparisonAlpha)
        {
            int redDifference = comparisonRed - testRed;
            if (redDifference < 0)
            {
                redDifference *= -1;
            }
            int greenDifference = comparisonGreen - testGreen;
            if (greenDifference < 0)
            {
                greenDifference *= -1;
            }
            int blueDifference = comparisonBlue - testBlue;
            if (blueDifference < 0)
            {
                blueDifference *= -1;
            }
            int alphaDifference = comparisonAlpha - testAlpha;
            if (alphaDifference < 0)
            {
                alphaDifference *= -1;
            }
            return redDifference + greenDifference + blueDifference + alphaDifference;
        }

        public static int difference(int testARGB, int comparisonARGB)
        {
            int difference = comparisonARGB - testARGB;
            if (difference < 0)
            {
                difference *= -1;
            }
            return difference;
        }

        #endregion Static Methods

        #region Class Methods

        public event ColorChanged OnColorChanged;
        public delegate void ColorChanged (object sender, ColorChangedEventArgs e);

        private float red = 0;
        private float green = 0;
        private float blue = 0;
        private float alpha = 255;
        private float hue = 0;
        private float saturation = 0;
        private float value = 0;
        public GokiColor(){}

        private void colorChanged()
        {
            if (OnColorChanged != null)
            {
                this.OnColorChanged(this, new ColorChangedEventArgs(this.Color));
            }
        }

        public Color Color
        {
            get
            {
                return Color.FromArgb((int)alpha, (int)red, (int)green, (int)blue);
            }
            set
            {
                Red = value.R;
                Green = value.G;
                Blue  = value.B;
                Alpha = value.A;
                colorChanged();
            }
        }

        public int ARGB
        {
            get
            {
                return ((byte)Alpha << 24) | ((byte)Red << 16) | ((byte)Green << 8) | ((byte)Blue);
            }
        }

        public float Red
        {
            get
            {
                return red;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 255)
                {
                    value = 255;
                }
                red = value;
                calculateHSV();
                colorChanged();
            }
        }

        public float Green
        {
            get
            {
                return green;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 255)
                {
                    value = 255;
                }
                green = value;
                calculateHSV();
                colorChanged();
            }
        }

        public float Blue
        {
            get
            {
                return blue;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 255)
                {
                    value = 255;
                }
                blue = value;
                calculateHSV();
                colorChanged();
            }
        }

        public float Alpha
        {
            get
            {
                return alpha;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 255)
                {
                    value = 255;
                }
                alpha = (byte)value;
                colorChanged();
            }
        }

        public float Hue
        {
            get
            {
                return hue;
            }

            set
            {
                if ( value < 0)
                {
                    value = 0;
                }
                else if ( value > 360)
                {
                    value = 360;
                }
                hue = value;
                calculateRGB();
            }
        }

        public float Saturation
        {
            get
            {
                return saturation;
            }
            set
            {
                if ( value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }
                saturation = value;
                calculateRGB();
            }
        }

        public float Value
        {
            get
            {
                return value;
            }
            set
            {
                if (value < 0)
                {
                    value = 0;
                }
                else if (value > 1)
                {
                    value = 1;
                }
                this.value = value;
                calculateRGB();
            }
        }

        private void calculateRGB()
        {
            GokiColor.HSVtoRGB(Hue, Saturation, Value, ref red, ref green, ref blue);
        }

        private void calculateHSV()
        {
            GokiColor.RGBtoHSV(Red, Green, Blue, ref hue, ref saturation, ref value);
        }

        public bool compare(GokiColor color)
        {
            return color.ARGB.Equals(ARGB);
        }

        public void interpolate(GokiColor color, float amount)
        {
            Red += (float)Math.Round((color.Red - Red) * amount);
            if ( Math.Abs(color.Red - Red) < color.Red * amount)
            {
                Red = color.Red;
            }
            Green += (float)Math.Round((color.Green - Green) * amount);
            if (Math.Abs(color.Green - Green) < color.Green * amount)
            {
                Green = color.Green;
            }
            Blue += (float)Math.Round((color.Blue - Blue) * amount);
            if (Math.Abs(color.Blue - Blue) < color.Blue * amount)
            {
                Blue = color.Blue;
            }
            Alpha += (float)Math.Round((color.Alpha - Alpha) * amount);
            if (Math.Abs(color.Alpha - Alpha) < color.Alpha * amount)
            {
                Alpha = color.Alpha;
            }
        }

        public int toInfo()
        {
            return GokiColor.infoFromARGB((byte)alpha, (byte)red, (byte)green,(byte)blue);
        }

        public GokiColor Copy
        {
            get
            {
                return GokiColor.fromARGB(Alpha, Red, Green, Blue);
            }
        }

        public void set(float red, float green, float blue, float alpha = 255)
        {
            red = (float)Math.Min(Math.Max(0, red), 255);
            green = (float)Math.Min(Math.Max(0, green), 255);
            blue = (float)Math.Min(Math.Max(0, blue), 255);
            alpha = (float)Math.Min(Math.Max(0, alpha), 255);
            colorChanged();
        }

        #endregion Class Methods

        public GokiColor multiply(float p)
        {
            Red *= p;
            Green *= p;
            Blue *= p;
            Alpha *= p;
            return this;
        }

        public GokiColor Inverse
        {
            get
            {
                return GokiColor.fromColor(GokiColor.inverse(Color));
            }
        }
    }

    public class GokiColorPalette
    {
        private byte[] data;
        private int length;

        public int Length
        {
            get { return length; }
        }
        private int capacity;

        public int Capacity
        {
            get { return capacity; }
        }

        public byte[] Data
        {
            get { return data; }
        }
        public GokiColorPalette(int capacity)
        {
            this.length = capacity * 4;
            this.capacity = capacity;
            this.data = new byte[length];
        }
        public GokiColorPalette(Color[] data)
        {
            this.capacity = data.Length;
            this.length = capacity * 4;
            this.data = new byte[length];
            for (int colorIndex = 0; colorIndex < data.Length; colorIndex++)
            {
                set(colorIndex, data[colorIndex]);
            }
        }
        public void set(int index, byte red, byte green, byte blue, byte alpha)
        {
            int dataIndex = index * 4;
            data[dataIndex + 0] = blue;
            data[dataIndex + 1] = green;
            data[dataIndex + 2] = red;
            data[dataIndex + 3] = alpha;
        }
        public void set(int index, Color color)
        {
            set(index, color.R, color.G, color.B, color.A);
        }

        public Color[] Colors
        {
            get
            {
                Color[] colors = new Color[Capacity];
                int colorIndex = 0;
                for (int i = 0; i < data.Length; i += 4)
                {
                    colors[colorIndex] = Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i + 0]);
                    colorIndex++;
                }
                return colors;
            }
        }
    }
}
