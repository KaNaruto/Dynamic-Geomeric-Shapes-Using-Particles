using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public class TorusKnot : GeometricObject
    {
        private const int P = 3;
        private const int Q = 8;

        public override float3[] GetPoints()
        {
        
            float3[] positions = new float3[DotAmount];
            for (int i = 0; i < DotAmount; i++)
            {
                float u = 2 * Mathf.PI * HaltonSequence(i, 2);
                float r = Mathf.Cos(Q * u) + 4;

                float x = r * Mathf.Cos(P * u);
                float y = r * Mathf.Sin(P * u);
                float z = -Mathf.Sin(Q * u);

                positions[i] = new Vector3(x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}
