using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore {
    public class RecvBuffer {
        private byte[] _buffer;
        private int _write, _read;
        private object _lock = new object();

        public RecvBuffer(int bufferSize) {
            _buffer = new byte[bufferSize];
            _write = _read = 0;
        }

        private int FreeSize { get { return _buffer.Length - _read; } }
        private int DataSize { get { return _write - _read; } }

        public ArraySegment<byte> WriteSegment { get { return new ArraySegment<byte>(_buffer, _write, FreeSize); } }
        public ArraySegment<byte> DataSegment { get { return new ArraySegment<byte>(_buffer, _read, DataSize); } }

        public bool OnWrite(int transferred) {
            if(transferred > FreeSize)
                return false;

            _write += transferred;
            return true;
        }

        public bool OnRead(int transferred) {
            int dataSize = DataSize;
            if(transferred != DataSize)
                return false;

            _read += transferred;
            return true;
        }

        public void Clear() {
            lock(_lock) {
                if(DataSize == 0) {
                    _read = _write = 0;
                }
                else {
                    int dataSize = DataSize;
                    Array.Copy(_buffer, _write, _buffer, 0, dataSize);
                    _read = 0; _write = dataSize;
                }
            }
        }
    }
}
