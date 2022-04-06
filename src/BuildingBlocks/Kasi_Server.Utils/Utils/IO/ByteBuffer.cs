namespace Kasi_Server.Utils.IO
{
    public class ByteBuffer
    {
        private byte[] _buffer;

        private int _readIndex = 0;

        private int _writeIndex = 0;

        private int _markReadIndex = 0;

        private int _markWriteIndex = 0;

        private int _capacity;

        private ByteBuffer(int capacity)
        {
            _buffer = new byte[capacity];
            this._capacity = capacity;
        }

        private ByteBuffer(byte[] bytes)
        {
            _buffer = bytes;
            this._capacity = bytes.Length;
        }

        public static ByteBuffer Allocate(int capacity) => new ByteBuffer(capacity);

        public static ByteBuffer Allocate(byte[] bytes) => new ByteBuffer(bytes);

        private int FixLength(int length)
        {
            var n = 2;
            var b = 2;
            while (b < length)
            {
                b = 2 << n;
                n++;
            }

            return b;
        }

        private byte[] Flip(byte[] bytes, bool isLittleEndian = false)
        {
            if (BitConverter.IsLittleEndian)
            {
                if (!isLittleEndian)
                    Array.Reverse(bytes);
            }
            else
            {
                if (isLittleEndian)
                    Array.Reverse(bytes);
            }

            return bytes;
        }

        private int FixSizeAndReset(int currLen, int futureLen)
        {
            if (futureLen > currLen)
            {
                var size = FixLength(currLen) * 2;
                if (futureLen > size)
                {
                    size = FixLength(futureLen) * 2;
                }

                byte[] newBuffer = new byte[size];
                Array.Copy(_buffer, 0, newBuffer, 0, currLen);
                _buffer = newBuffer;
                _capacity = newBuffer.Length;
            }

            return futureLen;
        }

        public void WriteBytes(byte[] bytes, int startIndex, int length)
        {
            lock (this)
            {
                var offset = length - startIndex;
                if (offset <= 0)
                    return;
                var total = offset + _writeIndex;
                var len = _buffer.Length;
                FixSizeAndReset(len, total);
                for (int i = _writeIndex, j = startIndex; i < total; i++, j++)
                {
                    _buffer[i] = bytes[j];
                }

                _writeIndex = total;
            }
        }

        public void WriteBytes(byte[] bytes, int length)
        {
        }
    }
}