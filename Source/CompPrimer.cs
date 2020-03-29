using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace Gunplay
{
    public abstract class CompPrimer : ThingComp
    {
        public enum State
        {
            Idle = 0,
            Priming = 1,
            Primed = 2
        };

        int previousTick = 0;
        public State state = State.Idle;

        public float position = 0;
        float targetPosition;
        float speed = 0;
        int movementTicksRemaing = 0;
    
        public virtual int TicksToIdle => 30;

        public void ReachPosition(float target, int ticksUntil)
        {
            targetPosition = target;

            if (ticksUntil <= 0)
            {
                movementTicksRemaing = 0;
                position = target;
            }

            movementTicksRemaing = ticksUntil;
            speed = (target - position) / ticksUntil;
        }

        bool IsBrusting(Pawn pawn)
        {
            if (pawn.CurrentEffectiveVerb == null) return false;
            return pawn.CurrentEffectiveVerb.Bursting;
        }

        public void UpdateState()
        {
            var holder = ParentHolder as Pawn_EquipmentTracker;
            if (holder == null) return;

            Stance stance = holder.pawn.stances.curStance;
            Stance_Warmup warmup;

            switch (state)
            {
                case State.Idle:
                    warmup = stance as Stance_Warmup;
                    if (warmup != null)
                    {
                        state = State.Priming;
                        ReachPosition(1.0f, warmup.ticksLeft);
                    }
                    break;
                case State.Priming:
                    if (IsBrusting(holder.pawn))
                    {
                        state = State.Primed;
                    }
                    else
                    {
                        warmup = stance as Stance_Warmup;
                        if (warmup == null)
                        {
                            state = State.Idle;
                            ReachPosition(0.0f, TicksToIdle);
                        }
                    }
                    break;
                case State.Primed:
                    if (!IsBrusting(holder.pawn))
                    {
                        state = State.Idle;
                        Stance_Cooldown cooldown = stance as Stance_Cooldown;
                        if (cooldown != null)
                            ReachPosition(0.0f, TicksToIdle);
                        else
                            ReachPosition(0.0f, 0);
                    }
                    break;
            }
        }

        public static Matrix4x4 drawingMatrix = default;
        public virtual void Draw(Mesh mesh, Vector3 drawLoc, float angle, Vector3 drawingScale)
        {
            drawingMatrix.SetTRS(drawLoc, Quaternion.AngleAxis(angle, Vector3.up), drawingScale);
            Graphics.DrawMesh(mesh, drawingMatrix, Graphic.MatSingle, 0);
        }

        public abstract Graphic GetGraphic(int ticksPassed);


        private Graphic GetGraphicForTick(int ticksPassed)
        {
            if (movementTicksRemaing > 0)
            {
                if (ticksPassed > movementTicksRemaing)
                    ticksPassed = movementTicksRemaing;

                movementTicksRemaing -= ticksPassed;
                position += ticksPassed * speed;

                if (movementTicksRemaing <= 0)
                {
                    position = targetPosition;
                }
            }

            return GetGraphic(ticksPassed);
        }

        public Graphic Graphic
        {
            get
            {
                UpdateState();

                var tick = Find.TickManager.TicksGame;
                var res = GetGraphicForTick(tick - previousTick);
                previousTick = tick;

                return res;
            }
        }
    }
}
