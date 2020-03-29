using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay
{
    class CompSpinningGun : CompPrimer
    {
        new CompPropertiesSpinningGun props => base.props as CompPropertiesSpinningGun;
        public override int TicksToIdle => props.ticksToIdle;

        float rotation = 0;

        public override Graphic GetGraphic(int ticksPassed)
        {
            rotation += position * props.rotationSpeed * ticksPassed;

            int frame = ((int)rotation) % props.frames.Count;
            return props.frames[frame].Graphic;
        }
    }
}
