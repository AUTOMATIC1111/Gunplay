using UnityEngine;
using Verse;

namespace Gunplay
{

    public class CompPrimerBow : CompPrimer
    {
        new CompPropertiesPrimerBow props => base.props as CompPropertiesPrimerBow;

        public override int TicksToIdle => 4;
        public override Graphic GetGraphic(int ticksPassed)
        {
            return props.graphicData == null ? parent.DefaultGraphic : props.graphicData.Graphic;
        }

        public override void Draw(Mesh mesh, Vector3 drawLoc, float angle, Vector3 drawingScale)
        {
            drawingScale.x *= 1.0f + position * 0.45f;
            drawingScale.z *= 1.0f - position * 0.25f;
            drawingMatrix.SetTRS(drawLoc, Quaternion.AngleAxis(angle, Vector3.up), drawingScale);
            Graphics.DrawMesh(mesh, drawingMatrix, Graphic.MatSingle, 0);
        }
    }
}