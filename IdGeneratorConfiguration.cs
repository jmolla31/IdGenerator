using System;
using System.Collections.Generic;
using System.Text;

namespace jmollami.IdGenerator
{
    public class IdGeneratorConfiguration
    {
        public int TimestampResolution { get; set; }
        
        public byte GeneratorId { get; set; }
        
        public int SubtickResolution { get; set; }

        public bool JavascriptCompatibility { get; set; }    
    }
}
