using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace Gunplay.Patch
{


    [HarmonyPatch(typeof(Projectile), "Launch", new Type[] { typeof(Thing), typeof(Vector3), typeof(LocalTargetInfo), typeof(LocalTargetInfo), typeof(ProjectileHitFlags), typeof(bool), typeof(Thing), typeof(ThingDef) })]
    class PatchProjectileLaunch
    {
        static PropertyInfo StartingTicksToImpactProp = typeof(Projectile).GetProperty("StartingTicksToImpact", BindingFlags.NonPublic | BindingFlags.Instance);

        static void Postfix(Projectile __instance, Vector3 ___destination, Thing launcher, ref Vector3 ___origin, LocalTargetInfo intendedTarget, Thing equipment, ref int ___ticksToImpact)
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
                ___ticksToImpact = Mathf.CeilToInt((float) StartingTicksToImpactProp.GetValue(__instance));
                if (___ticksToImpact < 1) ___ticksToImpact = 1;
            }

            if (Gunplay.settings.enableTrails && launcher?.Map!=null)
            {
                ProjectileTrail trail = GenSpawn.Spawn(prop.trail, ___origin.ToIntVec3(), launcher.Map, WipeMode.Vanish) as ProjectileTrail;
                trail.Initialize(__instance, ___destination, equipment);
            }
        }
    }

    [HarmonyPatch(typeof(Projectile), "get_StartingTicksToImpact")]
    class PatchProjectileStartingTicksToImpact
    {
        static float Postfix(float value, Projectile __instance)
        {
            GunPropDef prop = GunplaySetup.GunProp(__instance.EquipmentDef);
            if (prop == null || prop.preserveSpeed) return value;

            return value / Gunplay.settings.projectileSpeed;
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
