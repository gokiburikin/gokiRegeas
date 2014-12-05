using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GokiLibrary
{
    public class GokiBytesWriter
    {
        int index = 0;
        public byte[] data;
        public GokiBytesWriter(long capacity)
        {
            data = new byte[capacity];
        }

        private void add(byte data)
        {
            this.data[index++] = data;
        }

        private void add(byte[] data)
        {
            Array.Copy(data, 0, this.data, index, data.Length);
            index += data.Length;
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

        public void writeLong(long value)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            add(bytes);
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

        public void writeSizedByteArray( byte[] data )
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

        public void write(Color color)
        {
            writeColor(color);
        }

        public void write(long value)
        {
            writeLong(value);
        }

        #endregion

    }
}
