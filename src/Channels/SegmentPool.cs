// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Channels
{
    internal class SegmentPool : ISegmentPool
    {
        private readonly ObjectPool<PooledBufferSegment> _segmentPool;

        public SegmentPool()
        {
            _segmentPool = new ObjectPool<PooledBufferSegment>(() => new PooledBufferSegment(), Environment.ProcessorCount * 4);
        }

        public PooledBufferSegment Lease(PooledBuffer buffer)
        {
            var segment = _segmentPool.Allocate();
            segment.Initialize(buffer);
            return segment;
        }

        public PooledBufferSegment Lease(PooledBuffer buffer, int start, int end)
        {
            var segment = _segmentPool.Allocate();
            segment.Initialize(buffer, start, end);
            return segment;
        }

        public void Return(PooledBufferSegment segment)
        {
            _segmentPool.Free(segment);
        }
    }
}