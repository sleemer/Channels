using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Channels
{
    public struct Memory
    {
        private readonly byte[] _array;
        private readonly int _offset;
        private readonly unsafe byte* _memory;
        private readonly int _memoryLength;

        public Memory(ArraySegment<byte> segment)
        {
            _array = segment.Array;
            _offset = segment.Offset;
            unsafe
            {
                _memory = null;
            }

            _memoryLength = segment.Count;
        }

        public unsafe Memory(byte* pointer, int length)
        {
            unsafe
            {
                _memory = pointer;
            }

            _array = null;
            _offset = 0;
            _memoryLength = length;
        }

        public unsafe Memory(byte[] array, int offset, int length, byte* pointer = null)
        {
            unsafe
            {
                _memory = pointer;
            }

            _array = array;
            _offset = offset;
            _memoryLength = length;
        }

        public Span<byte> Span => this;

        public static implicit operator Span<byte>(Memory memory)
        {
            if (memory.Length == 0)
            {
                return Span<byte>.Empty;
            }

            if (memory._array != null)
            {
                return memory._array.Slice(memory._offset, memory.Length);
            }
            else
            {
                unsafe
                {
                    return new Span<byte>(memory._memory, memory._memoryLength);
                }
            }
        }

        public unsafe byte* UnsafePointer => _memory + _offset;

        public int Length => _memoryLength;

        public unsafe Memory Slice(int offset, int length)
        {
            // TODO: Bounds check
            if (_array == null)
            {
                return new Memory(_memory + offset, length);
            }

            return new Memory(_array, _offset + offset, length, _memory);
        }

        public bool TryGetArray(out ArraySegment<byte> buffer)
        {
            if (_array == null)
            {
                buffer = default(ArraySegment<byte>);
                return false;
            }
            buffer = new ArraySegment<byte>(_array, _offset, Length);
            return true;
        }
    }
}
