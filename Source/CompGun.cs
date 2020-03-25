using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    class CompGun : ThingComp
    {
        float rotationSpeed = 0;
        float rotationOffset = 0;
        int ticksPreviously = 0;

        void UpdateRotationOffset(int ticks)
        {
            if (rotationOffset == 0) return;
            if (ticks <= 0) return;
            if (ticks > 30) ticks = 30;

            if (rotationOffset > 0)
            {
                rotationOffset -= rotationSpeed;
                if (rotationOffset < 0) rotationOffset = 0;
            }
            else if (rotationOffset < 0)
            {
                rotationOffset += rotationSpeed;
                if (rotationOffset > 0) rotationOffset = 0;
            }

            rotationSpeed += ticks * 0.01f;
        }

        public float RotationOffset
        {
            get
            {
                int ticks = Find.TickManager.TicksGame;
                UpdateRotationOffset(ticks - ticksPreviously);
                ticksPreviously = ticks;

                return rotationOffset;
            }
            set
            {
                rotationOffset = value;
                rotationSpeed = 0;
            }
        }
    }
}
