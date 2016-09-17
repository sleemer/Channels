// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Channels
{
    internal interface ISegmentPool
    {
        PooledBufferSegment Lease(PooledBuffer buffer);

        PooledBufferSegment Lease(PooledBuffer buffer, int start, int end);

        void Return(PooledBufferSegment segment);
    }
}