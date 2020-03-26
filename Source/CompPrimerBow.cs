using UnityEngine;
using Verse;

namespace Gunplay
{

    public class CompPrimerBow : CompPrimer
    {
        new CompPropertiesPrimerBow props => base.props as CompPropertiesPrimerBow;
        static Vector3 dir = new Vector3(1f, 0f, 0f);

        public override int TicksToIdle => 4;
        public override Graphic GetGraphic(int ticksPassed)
        {
            return props.graphicData == null ? parent.DefaultGraphic : props.graphicData.Graphic;
        }

        public override void Draw(Mesh mesh, Vector3 drawLoc, float angle, Vector3 drawingScale)
        {
            float w = drawingScale.x;
            drawingScale.x *= 1f + position * props.stretchHorizontal;
            drawingScale.z *= 1f + position * props.stretchVertical;
            drawLoc += (mesh == MeshPool.plane10 ? -1f : 1f) *  w * position * props.stretchHorizontal * 0.25f * dir.RotatedBy(angle);

            drawingMatrix.SetTRS(drawLoc, Quaternion.AngleAxis(angle, Vector3.up), drawingScale);
            Graphics.DrawMesh(mesh, drawingMatrix, Graphic.MatSingle, 0);
        }
    }
}