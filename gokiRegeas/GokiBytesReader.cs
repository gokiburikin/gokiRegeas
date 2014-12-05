using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace GokiLibrary
{
    public class GokiBytesReader
    {
        int index = 0;

        public int Index
        {
            get { return index; }
            set { index = value; }
        }

        byte[] data;
        public GokiBytesReader(byte[] data)
        {
            this.data = data;
        }

        public bool EOF
        {
            get
            {
                if ( index >= data.Length)
                {
                    return true;
                }
                return false;
            }
        }

        #region Read

        public byte read()
        {
            byte output = 0;
            if (index  < data.Length)
            {
                output = data[index++];
            }
            else
            {
                throw new IndexOutOfRangeException("Data array was not long enough.");
            }
            return output;
        }

        public void read(byte[] data)
        {
            Array.Copy(this.data, index, data, 0, data.Length);
            index += data.Length;
        }

        public byte[] readByteArray(byte[] data)
        {
            Array.Copy(this.data, index, data, 0, data.Length);
            index += data.Length;
            return data;
        }

        public byte[] readSizedByteArray()
        {
            byte[] data = new byte[readInt()];
            Array.Copy(this.data, index, data, 0, data.Length);
            index += data.Length;
            return data;
        }

        public Color readColor()
        {
            Color output = Color.Empty;
            byte[] colorInfo = new byte[4];
            read(colorInfo);
            output = GokiColor.fromInfo(colorInfo);
            return output;
        }

        public Int32 readInt()
        {
            Int32 output = 0;
            byte[] intBytes = new byte[sizeof(Int32)];
            read(intBytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(intBytes);
            }
            output = BitConverter.ToInt32(intBytes, 0);
            return output;
        }

        public Int64 readLong()
        {
            Int64 output = 0;
            byte[] longBytes = new byte[sizeof(Int64)];
            read(longBytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(longBytes);
            }
            output = BitConverter.ToInt64(longBytes, 0);
            return output;
        }

        public float readFloat()
        {
            float output = 0;
            byte[] bytes = new byte[sizeof(float)];
            read(bytes);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            output = BitConverter.ToSingle(bytes, 0);
            return output;
        }

        public bool readBoolean()
        {
            bool output = false;
            output = Convert.ToBoolean(read());
            return output;
        }

        public string readString()
        {
            string output = String.Empty;
            byte[] stringData = new byte[readInt()];
            read(stringData);
            output = GokiUtility.byteArrayToString(stringData);
            return output;
        }

        #endregion

    }
}
