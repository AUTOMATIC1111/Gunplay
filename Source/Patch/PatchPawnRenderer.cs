using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay.Patch
{

    [HarmonyPatch(typeof(PawnRenderUtility), "DrawEquipmentAiming", new Type[] { typeof(Thing), typeof(Vector3), typeof(float) })]
    class PatchPawnRenderUtilityDrawEquipmentAiming
    {
        static FieldInfo pawnField = typeof(PawnRenderer).GetField("pawn", BindingFlags.NonPublic | BindingFlags.Instance);
        static Vector3 equipmentDir = new Vector3(0f, 0f, 0.4f);
        static Vector3 drawingScale = new Vector3(1f, 1f, 1f);
        static Matrix4x4 drawingMatrix = default;

        static bool Prefix(ref Thing eq, ref Vector3 drawLoc, ref float aimAngle)
        {
            if (!Gunplay.settings.enableWeaponAnimations) return true;

            CompGun comp = eq.TryGetComp<CompGun>();
            if (comp == null) return true;

            drawLoc -= equipmentDir.RotatedBy(aimAngle);
            aimAngle = (aimAngle + comp.RotationOffset) % 360;
            drawLoc += equipmentDir.RotatedBy(aimAngle);

            GunPropDef prop = GunplaySetup.GunProp(eq);
            if (prop == null) return true;

            float num = aimAngle - 90f;
            Mesh mesh;
            if (aimAngle > 20f && aimAngle < 160f)
            {
                mesh = MeshPool.plane10;
                num += eq.def.equippedAngleOffset;
            }
            else if (aimAngle > 200f && aimAngle < 340f)
            {
                mesh = MeshPool.plane10Flip;
                num -= 180f;
                num -= eq.def.equippedAngleOffset;
            }
            else
            {
                mesh = MeshPool.plane10;
                num += eq.def.equippedAngleOffset;
            }
            num %= 360f;

            drawingScale.x = drawingScale.z = prop.drawScale;
            CompPrimer primer = eq.TryGetComp<CompPrimer>();
            if (primer != null)
            {
                primer.Draw(mesh, drawLoc, num, drawingScale);
                return false;
            }
            
            if(prop.drawScale == 1f) return true;

            Material mat;

            Graphic_StackCount graphic_StackCount = eq.Graphic as Graphic_StackCount;
            if (graphic_StackCount != null)
            {
                mat = graphic_StackCount.SubGraphicForStackCount(1, eq.def).MatSingle;
            }
            else
            {
                mat = eq.Graphic.MatSingle;
            }

            drawingMatrix.SetTRS(drawLoc, Quaternion.AngleAxis(num, Vector3.up), drawingScale);
            Graphics.DrawMesh(mesh, drawingMatrix, mat, 0);

            return false;
        }


    }
}
