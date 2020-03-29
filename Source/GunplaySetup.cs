using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace Gunplay
{
    [StaticConstructorOnStartup]
    public class GunplaySetup
    {
        static Dictionary<ThingDef, GunPropDef> propMap = new Dictionary<ThingDef, GunPropDef>();
        static GunPropDef defaultDef = DefDatabase<GunPropDef>.GetNamed("default");

        static GunplaySetup()
        {
            var harmony = new Harmony("com.github.automatic1111.gunplay");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            foreach (ProjectileTrailDef def in DefDatabase<ProjectileTrailDef>.AllDefs)
            {
                if (!def.DrawMatSingle || !def.DrawMatSingle.mainTexture) continue;

                def.DrawMatSingle.mainTexture.wrapMode = TextureWrapMode.Clamp;
            }

            foreach (GunPropDef def in DefDatabase<GunPropDef>.AllDefs)
            {
                ThingDef target = null;

                if (def.defTarget != null) target = DefDatabase<ThingDef>.GetNamed(def.defTarget, false);
                if (target == null) target = DefDatabase<ThingDef>.GetNamed(def.defName, false);
                if (target != null) propMap[target] = def;

                if (def.trail == null) def.trail = defaultDef.trail;
                if (def.barrelLength == -1) def.barrelLength = defaultDef.barrelLength;

                if (Gunplay.settings.enableSounds)
                {
                    if (def.projectileImpactSound == null) def.projectileImpactSound = defaultDef.projectileImpactSound;
                    if (def.projectileImpactEffect == null) def.projectileImpactEffect = defaultDef.projectileImpactEffect;
                }
            }

            foreach (ThingDef def in DefDatabase<ThingDef>.AllDefs)
            {
                VerbProperties shoot = def.Verbs.FirstOrDefault(v => typeof(Verb_Shoot).IsAssignableFrom(v.verbClass));
                if (shoot == null) continue;
                if (!propMap.ContainsKey(def)) propMap[def] = defaultDef;

                def.comps.Add(new CompProperties() { compClass = typeof(CompGun) });

                GunPropDef prop = propMap.TryGetValue(def);
                if (prop != null)
                {
                    if (prop.soundAiming != null) shoot.soundAiming = prop.soundAiming;
                    if (prop.soundCast != null) shoot.soundCast = prop.soundCast;
                    if (prop.spinner != null) def.comps.Add(prop.spinner);
                    if (prop.primer != null) def.comps.Add(prop.primer);
                    if (prop.bow != null) def.comps.Add(prop.bow);
                }
            }
        }

        public static GunPropDef GunProp(ThingDef equipment)
        {
            if (equipment == null) return null;

            return propMap.TryGetValue(equipment, null);
        }

        public static GunPropDef GunProp(Thing equipment)
        {
            if (equipment?.def == null) return null;

            return propMap.TryGetValue(equipment.def, defaultDef);
        }
    }
}
