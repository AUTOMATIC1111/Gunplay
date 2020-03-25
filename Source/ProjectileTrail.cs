using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay
{
    class ProjectileTrail : ThingWithComps
    {
        Matrix4x4 drawingMatrix = default;

        int ticksUtilDeath;
        int ticksIdle;
        Projectile projectile;
        Vector3 a;
        Vector3 exactPosition;
        Vector3 previousPosition;
        float speed;
        Vector3 dir;
        float length;
        float width;

        public void Initialize(Projectile proj, Vector3 destination, Thing equipment)
        {
            ticksUtilDeath = -1;
            ticksIdle = 0;
            projectile = proj;
            a = projectile.ExactPosition;
            Vector3 b = destination;
            a.y = b.y = proj.def.Altitude;
            speed = proj.def.projectile.SpeedTilesPerTick;
            exactPosition = a;

            length = speed * 15f;
            dir = (b - a).normalized;
            width = proj.DamageAmount * 0.006f;


            ProjectileTrailDef trailDef = def as ProjectileTrailDef;
            if(trailDef != null && trailDef.trailWidth != -1) width = trailDef.trailWidth;
        }


        public override void ExposeData()
        {
            base.ExposeData();

            Scribe_References.Look(ref projectile, "projectile");
            Scribe_Values.Look(ref a, "a");
            Scribe_Values.Look(ref speed, "speed");
            Scribe_Values.Look(ref dir, "dir");
            Scribe_Values.Look(ref width, "width");
            Scribe_Values.Look(ref exactPosition, "exactPosition");
            Scribe_Values.Look(ref ticksUtilDeath, "ticksUtilDeath");
        }

        public override Vector3 DrawPos
        {
            get
            {
                return exactPosition;
            }
        }

        float TicksPerLength => length / speed;

        public override void Tick()
        {
            base.Tick();

            if (projectile != null)
            {
                exactPosition = projectile.ExactPosition;
                exactPosition.y = projectile.def.Altitude;

                if (projectile.Destroyed || (ticksUtilDeath == -1 && ticksIdle>10 && previousPosition == exactPosition))
                {
                    projectile = null;
                    ticksUtilDeath = Mathf.CeilToInt(TicksPerLength);
                }
            }

            if (previousPosition == exactPosition)
                ticksIdle++;

            previousPosition = exactPosition;

            Position = exactPosition.ToIntVec3();

            if (ticksUtilDeath > 0)
                ticksUtilDeath--;
            else if(ticksUtilDeath == 0)
                Destroy(DestroyMode.Vanish);
        }

        public override void Draw()
        {
            float traveled = (a - exactPosition).magnitude;
            float len;
            Vector3 drawPos;
            if (traveled < length)
            {
                len = traveled;
                if (ticksUtilDeath == -1)
                {
                    drawPos = (a + exactPosition) / 2;
                }
                else
                {
                    len *= ticksUtilDeath / TicksPerLength;
                    drawPos = exactPosition - dir * len / 2;
                }
            }
            else if (ticksUtilDeath != -1)
            {
                len = length * ticksUtilDeath / TicksPerLength;
                drawPos = exactPosition - dir * len / 2;
            }
            else
            {
                len = length;
                drawPos = exactPosition - dir * len / 2;
            }

            Vector3 drawingScale = new Vector3(width, 1f, len);
            drawingMatrix.SetTRS(drawPos, Quaternion.LookRotation(dir), drawingScale);
            Graphics.DrawMesh(MeshPool.plane10, drawingMatrix, Graphic.MatSingle, 0);

            Comps_PostDraw();
        }

    }
}
