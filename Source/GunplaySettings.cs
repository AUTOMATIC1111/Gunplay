using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
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

        public void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("GunplayEnableTrailsName".Translate(), ref enableTrails, "GunplayEnableTrailsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableSoundsName".Translate(), ref enableSounds, "GunplayEnableSoundsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableWeaponAnimationsName".Translate(), ref enableWeaponAnimations, "GunplayEnableWeaponAnimationsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableEffectsName".Translate(), ref enableEffects, "GunplayEnableEffectsDesc".Translate());
            listing_Standard.SliderLabeled("GunplayProjectileSpeedName".Translate(), ref projectileSpeed, "GunplayProjectileSpeedDesc".Translate(), 0.1f, 10, projectileSpeed.ToStringPercent());
            listing_Standard.End();
        }
    }

}
