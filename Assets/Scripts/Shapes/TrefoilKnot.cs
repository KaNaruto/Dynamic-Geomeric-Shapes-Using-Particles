using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public class TrefoilKnot : GeometricObject
    {
        public  override float3[] GetPoints()
        {
            float3[] positions = new float3[DotAmount];
            for (int i = 0; i < DotAmount; i++)
            {
                float t = 2 * Mathf.PI * HaltonSequence(i, 2);

                float x = Mathf.Sin(t) + 2 * Mathf.Sin(2 * t);
                float y = Mathf.Cos(t) - 2 * Mathf.Cos(2 * t);
                float z = -Mathf.Sin(3 * t);
            
                positions[i] = new Vector3(x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}
