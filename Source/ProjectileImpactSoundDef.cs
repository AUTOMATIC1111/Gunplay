using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Gunplay
{
    public class ProjectileImpactSoundDef : Def
    {
        public SoundDef fallback;
        public SoundDef fabric;
        public SoundDef flesh;
        public SoundDef metal;
        public SoundDef soil;
        public SoundDef stone;
        public SoundDef wood;

        public SoundDef Effect(MaterialKind kind) {
            SoundDef res = null;

            switch (kind)
            {
                case MaterialKind.None: res = fallback; break;
                case MaterialKind.Fabric: res = fabric; break;
                case MaterialKind.Flesh: res = flesh; break;
                case MaterialKind.Metal: res = metal; break;
                case MaterialKind.Soil: res = soil; break;
                case MaterialKind.Stone: res = stone; break;
                case MaterialKind.Wood: res = wood; break;
            }

            if (res == null) res = fallback;

            return res;
        }
    }
}
