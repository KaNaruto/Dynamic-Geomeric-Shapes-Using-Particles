using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public class Torus : GeometricObject
    {
        public override float3[] GetPoints()
        {
            float majorRadius = 1.0f; 
            float minorRadius = 0.5f; 
            float3[] positions = new float3[DotAmount];
            for (int i = 0; i < DotAmount; i++)
            {
                float u = 2 * Mathf.PI * HaltonSequence(i, 2);
                float v = 2 * Mathf.PI * HaltonSequence(i, 3);

                float x = (majorRadius + minorRadius * Mathf.Cos(v)) * Mathf.Cos(u);
                float y = (majorRadius + minorRadius * Mathf.Cos(v)) * Mathf.Sin(u);
                float z = minorRadius * Mathf.Sin(v);

                positions[i] = new Vector3(x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}