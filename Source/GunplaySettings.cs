using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    public class GunplaySettings : ModSettings
    {
        public bool enableTrails = true;
        public bool enableSounds = true;
        public bool enableWeaponAnimations = true;
        public bool enableEffects = true;
        public float projectileSpeed = 3f;


        override public void ExposeData()
        {
            Scribe_Values.Look(ref enableTrails, "enableTrails");
            Scribe_Values.Look(ref enableSounds, "enenableSoundsableTrails");
            Scribe_Values.Look(ref enableWeaponAnimations, "enableWeaponAnimations");
            Scribe_Values.Look(ref enableEffects, "enableEffects");
            Scribe_Values.Look(ref projectileSpeed, "projectileSpeed");
        }
    }

}
