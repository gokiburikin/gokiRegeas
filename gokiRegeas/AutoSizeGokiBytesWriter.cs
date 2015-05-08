using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GokiLibrary
{
    public class AutoSizeGokiBytesWriter
    {
        public int Index
        {
            get { return data.Count; }
        }

        private List<byte> data;
        public AutoSizeGokiBytesWriter()
        {
            data = new List<byte>(50);
        }

        public AutoSizeGokiBytesWriter(byte[] data)
        {
            this.data = new List<byte>(data.Length * 2);
            data.CopyTo(data, 0);
        }

        private void add(byte data)
        {
            this.data.Add( data);
        }

        private void add(byte[] data)
        {
            this.data.AddRange(data);
        }

        #region Write

        public void writeInt(Int32 value)
        {
            byte[] intBytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            add(intBytes);
        }

        public void writeColor(Color color)
        {
            add(GokiColor.extractInfo(color));
        }

        public void writeFloat(float value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            add(bytes);
        }

        public void writeDouble(double value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            add(bytes);
        }

        public void writeBoolean(bool value)
        {
            add(Convert.ToByte(value));
        }

        public void writeString(string value)
        {
            byte[] stringData = GokiUtility.stringToByteArray(value);
            writeInt(stringData.Length);
            add(stringData);
        }

        public void writeSizedByteArray(byte[] data)
        {
            writeInt(data.Length);
            add(data);
        }

        public void write(byte[] data)
        {
            add(data);
        }

        public void write(bool value)
        {
            writeBoolean(value);
        }

        public void write(string value)
        {
            writeString(value);
        }

        public void write(int value)
        {
            writeInt(value);
        }

        public void write(float value)
        {
            writeFloat(value);
        }

        public void write(double value)
        {
            writeDouble(value);
        }

        public void write(Color color)
        {
            writeColor(color);
        }

        #endregion


        public byte[] Data
        {
            get
            {
                return data.ToArray();
            }
        }
    }
}
