using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay
{
    class CompPrimeAnimation : CompPrimer
    {
        new CompPropertiesPrimeAnimation props => base.props as CompPropertiesPrimeAnimation;

        public override int TicksToIdle => props.ticksToIdle;

        public override Graphic GetGraphic(int ticksPassed)
        {
            int frame = Mathf.FloorToInt(props.frames.Count * position * 0.99999f);
            return props.frames[frame].Graphic;
        }
    }
}
