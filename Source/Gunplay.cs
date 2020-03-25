using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay
{
    public class Gunplay : Mod
    {
        public static GunplaySettings settings;

        public Gunplay(ModContentPack pack) : base(pack)
        {
            settings = GetSettings<GunplaySettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Listing_Standard listing_Standard = new Listing_Standard();
            listing_Standard.Begin(inRect);
            listing_Standard.CheckboxLabeled("GunplayEnableTrailsName".Translate(), ref settings.enableTrails, "GunplayEnableTrailsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableSoundsName".Translate(), ref settings.enableSounds, "GunplayEnableSoundsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableWeaponAnimationsName".Translate(), ref settings.enableWeaponAnimations, "GunplayEnableWeaponAnimationsDesc".Translate());
            listing_Standard.CheckboxLabeled("GunplayEnableEffectsName".Translate(), ref settings.enableEffects, "GunplayEnableEffectsDesc".Translate());
            listing_Standard.SliderLabeled("GunplayProjectileSpeedName".Translate(), ref settings.projectileSpeed, "GunplayProjectileSpeedDesc".Translate(), 0.1f, 10, settings.projectileSpeed.ToStringPercent());
            listing_Standard.End();
        }

        public override string SettingsCategory()
        {
            return "GunplayTitle".Translate();
        }
    }
}
