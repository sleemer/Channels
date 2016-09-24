using System;
using System.Collections;
using System.Collections.Generic;

namespace Channels
{
    /// <summary>
    /// An enumerator over the <see cref="ReadableBuffer"/>
    /// </summary>
    public struct SpanEnumerator : IEnumerator<Span<byte>>
    {
        private MemoryEnumerator _enumerator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="buffer"></param>
        public SpanEnumerator(ref ReadableBuffer buffer)
        {
            _enumerator = new MemoryEnumerator(ref buffer);
        }

        /// <summary>
        /// The current <see cref="Span{Byte}"/>
        /// </summary>
        public Span<byte> Current => _enumerator.Current;

        object IEnumerator.Current
        {
            get { return _enumerator.Current; }
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
            return _enumerator.MoveNext();
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