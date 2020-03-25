using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Gunplay.Patch
{


    [HarmonyPatch(typeof(Projectile), "Launch", new Type[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(Thing), typeof(ThingDef) })]
    class PatchProjectileLaunch
    {
        static void Postfix(Projectile __instance, Vector3 ___destination, ref float ___ticksToImpact, Thing launcher, ref Vector3 ___origin, LocalTargetInfo usedTarget, LocalTargetInfo intendedTarget, ProjectileHitFlags hitFlags, Thing equipment, ThingDef targetCoverDef)
        {
            GunPropDef prop = GunplaySetup.GunProp(equipment);
            if (prop == null) return;

            CompGun comp = equipment.TryGetComp<CompGun>();
            if (comp != null)
            {
                float angle = (___destination - ___origin).AngleFlat() - (intendedTarget.CenterVector3 - ___origin).AngleFlat();
                comp.RotationOffset = (angle + 180) % 360 - 180;
            }

            if (launcher as Pawn != null)
            {
                ___origin += (___destination - ___origin).normalized * prop.barrelLength;
            }

            if (Gunplay.settings.enableTrails)
            {
                ProjectileTrail trail = GenSpawn.Spawn(prop.trail, ___origin.ToIntVec3(), launcher.Map, WipeMode.Vanish) as ProjectileTrail;
                trail.Initialize(__instance, ___destination, equipment);
            }
        }
    }

    [HarmonyPatch(typeof(ProjectileProperties), "get_SpeedTilesPerTick")]
    class PatchProjectilePropertiesSpeedTilesPerTick
    {
        static float Postfix(float res)
        {
            return res * Gunplay.settings.projectileSpeed;
        }

    }

    [HarmonyPatch(typeof(Projectile), "Impact")]
    class PatchProjectileImpact
    {
        static void Prefix(Projectile __instance, Thing hitThing, Vector3 ___origin)
        {
            Map map = __instance.Map;
            if (map == null) return;

            GunPropDef prop = GunplaySetup.GunProp(__instance.EquipmentDef);
            if (prop == null) return;

            MaterialKind kind = MaterialKind.None;

            if (hitThing != null)
            {
                kind = MaterialKindGetter.Get(hitThing);
            }

            if(kind == MaterialKind.None)
            {
                TerrainDef terrainDef = map.terrainGrid.TerrainAt(CellIndicesUtility.CellToIndex(__instance.Position, map.Size.x));
                kind = MaterialKindGetter.Get(terrainDef);
            }

            if (Gunplay.settings.enableSounds) {
                SoundDef sound = prop.projectileImpactSound == null ? null : prop.projectileImpactSound.Effect(kind);
                if (sound != null) sound.PlayOneShot(new TargetInfo(__instance.Position, map, false));
            }

            if (Gunplay.settings.enableEffects)
            {
                EffecterDef effecterDef = prop.projectileImpactEffect == null ? null : prop.projectileImpactEffect.Effect(kind);
                if (effecterDef != null)
                {
                    Effecter effecter = new Effecter(effecterDef);
                    effecter.Trigger(__instance, new TargetInfo(___origin.ToIntVec3(), __instance.Map));
                    effecter.Cleanup();
                }
            }
        }
    }


}
