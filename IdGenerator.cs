using System;
using System.Collections.Generic;
using System.Text;

namespace jmollami.IdGenerator
{
    public class IdGenerator
    {
        private const int GeneratorIdResolution = 8;
        private readonly TickSource timeSource;

        private long lastTick = 0;
        private long seq = 0;

        private readonly int subtickResolution;
        private readonly byte generatorId;
        private readonly object @lock = new object();


        public IdGenerator(IdGeneratorConfiguration config)
        {
            var maxBits = (config.JavascriptCompatibility) ? 53 : 63;

            var configuredBits = (config.GeneratorId == 0)
                ? config.SubtickResolution + config.TimestampResolution
                : config.SubtickResolution + config.TimestampResolution + 8;

            if (configuredBits > maxBits) throw new Exception("Incorrect configuration provided, incorrect bits total");

            this.timeSource = new TickSource(config.TimestampResolution);
            this.subtickResolution = config.SubtickResolution;
            this.generatorId = config.GeneratorId;
        }

        public long GenerateId()
        {
            lock (@lock)
            {
                var currentTick = this.timeSource.GetTicks();

                if (currentTick < lastTick) throw new Exception("Clock moved back or wrapped around, refusing to generate a new id");

                var tick = currentTick;

                if (generatorId != 0) tick <<= GeneratorIdResolution;

                tick += this.generatorId;

                tick <<= this.subtickResolution;

                if (currentTick == lastTick)
                {
                    // TODO: Check for seq overflow
                    this.seq++;
                }
                else
                {
                    this.seq = 0;
                    this.lastTick = currentTick;
                }

                return tick + seq;
            }
        }
    }
}
