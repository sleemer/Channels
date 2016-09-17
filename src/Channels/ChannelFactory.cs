using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Channels
{
    /// <summary>
    /// Factory used to creaet instances of various channels.
    /// </summary>
    public class ChannelFactory : IDisposable
    {
        private readonly IBufferPool _pool;
        private readonly ISegmentPool _segmentPool;

        public ChannelFactory() : this(new MemoryPool())
        {
        }

        public ChannelFactory(MemoryPool pool) : this(pool, new SegmentPool())
        {

        }

        internal ChannelFactory(IBufferPool pool, ISegmentPool segmentPool)
        {
            _pool = pool;
            _segmentPool = segmentPool;
        }

        public Channel CreateChannel() => new Channel(_pool, _segmentPool);

        public IReadableChannel MakeReadableChannel(Stream stream)
        {
            if (!stream.CanRead)
            {
                throw new InvalidOperationException();
            }

            var channel = new Channel(_pool, _segmentPool);
            ExecuteCopyToAsync(channel, stream);
            return channel;
        }

        private async void ExecuteCopyToAsync(Channel channel, Stream stream)
        {
            await channel.ReadingStarted;

            await stream.CopyToAsync(channel);
        }

        public IChannel MakeChannel(Stream stream)
        {
            return new StreamChannel(this, stream);
        }

        public IWritableChannel MakeWriteableChannel(Stream stream)
        {
            if (!stream.CanWrite)
            {
                throw new InvalidOperationException();
            }

            var channel = new Channel(_pool, _segmentPool);

            channel.CopyToAsync(stream).ContinueWith((task) =>
            {
                if (task.IsFaulted)
                {
                    channel.CompleteReading(task.Exception);
                }
                else
                {
                    channel.CompleteReading();
                }
            });

            return channel;
        }

        public IWritableChannel MakeWriteableChannel(IWritableChannel channel, Func<IReadableChannel, IWritableChannel, Task> consume)
        {
            var newChannel = new Channel(_pool, _segmentPool);

            consume(newChannel, channel).ContinueWith(t =>
            {
            });

            return newChannel;
        }

        public IReadableChannel MakeReadableChannel(IReadableChannel channel, Func<IReadableChannel, IWritableChannel, Task> produce)
        {
            var newChannel = new Channel(_pool, _segmentPool);
            Execute(channel, newChannel, produce);
            return newChannel;
        }

        private async void Execute(IReadableChannel channel, Channel newChannel, Func<IReadableChannel, IWritableChannel, Task> produce)
        {
            await newChannel.ReadingStarted;

            await produce(channel, newChannel);
        }

        public void Dispose() => _pool.Dispose();
    }
}
