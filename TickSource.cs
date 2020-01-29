using System;

namespace jmollami.IdGenerator
{
    public class TickSource : ITickSource
    {
        private const int DefaultResolution = 43;

        private readonly TimeSpan TickDuration = TimeSpan.FromMilliseconds(1);
        private readonly int Resolution = DefaultResolution;
        private readonly DateTimeOffset Epoch = DateTimeOffset.UnixEpoch;

        private TimeSpan Elapsed { get { return DateTime.UtcNow - Epoch; } }


        public TickSource(DateTimeOffset epoch) => this.Epoch = epoch;

        public TickSource(int resolution) => this.Resolution = resolution;

        public TickSource(DateTimeOffset epoch, int resolution)
        {
            this.Epoch = epoch;
            this.Resolution = resolution;
        }

        public long GetTicks()
        {
            var currentTicks = this.Elapsed.Ticks / TickDuration.Ticks;

            var tickMask = (1L << this.Resolution) - 1;

            var maskedTicks = currentTicks & tickMask;

            return maskedTicks;
        }
    }
}

