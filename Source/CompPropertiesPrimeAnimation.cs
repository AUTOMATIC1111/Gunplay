using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    public class CompPropertiesPrimeAnimation : CompProperties
    {
        public CompPropertiesPrimeAnimation() { compClass = typeof(CompPrimeAnimation); }

        public List<GraphicData> frames;
        public int ticksToIdle = 0;
    }
}
