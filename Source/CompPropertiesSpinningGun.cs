using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    public class CompPropertiesSpinningGun : CompProperties
    {
        public CompPropertiesSpinningGun() { compClass = typeof(CompSpinningGun); }

        public List<GraphicData> frames;
        public float rotationSpeed = 1.0f;
        public int ticksToIdle = 60;
    }
}
