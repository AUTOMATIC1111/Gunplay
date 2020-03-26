using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    public class GunPropDef : Def
    {
        public ProjectileTrailDef trail;

        public CompPropertiesSpinningGun spinner;
        public CompPropertiesPrimeAnimation primer;
        public CompPropertiesPrimerBow bow;

        public SoundDef soundAiming;
        public SoundDef soundCast;

        public float barrelLength = -1;

        public float drawScale = 1;

        public ProjectileImpactSoundDef projectileImpactSound;
        public ProjectileImpactEffectDef projectileImpactEffect;
    }
}
