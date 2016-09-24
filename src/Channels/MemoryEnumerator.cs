using System;
using System.Collections;
using System.Collections.Generic;

namespace Channels
{
    /// <summary>
    /// An enumerator over the <see cref="ReadableBuffer"/>
    /// </summary>
    public struct MemoryEnumerator : IEnumerator<Memory>
    {
        private ReadableBuffer _buffer;
        private Memory _current;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public MemoryEnumerator(ref ReadableBuffer buffer)
        {
            _buffer = buffer;
            _current = default(Memory);
        }

        /// <summary>
        /// The current <see cref="Span{Byte}"/>
        /// </summary>
        public Memory Current => _current;

        object IEnumerator.Current
        {
            get { return _current; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {

        }

        /// <summary>
        /// Moves to the next <see cref="Span{Byte}"/> in the <see cref="ReadableBuffer"/>
        /// </summary>
        /// <returns></returns>
        public bool MoveNext()
        {
            var start = _buffer.Start;
            var moved = start.TryGetBuffer(_buffer.End, out _current, out start);
            _buffer = _buffer.Slice(start);
            return moved;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            throw new NotSupportedException();
        }
    }
}