using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public class Sphere : GeometricObject
    {
        public override float3[] GetPoints()
        {
            float3[] positions = new float3[DotAmount];

            for (int i = 0; i < DotAmount; i++)
            {
                float theta = Mathf.Acos(1 - 2 * HaltonSequence(i, 2)); // Polar angle
                float phi = 2 * Mathf.PI * HaltonSequence(i, 3); // Azimuthal angle

                float x = Mathf.Sin(theta) * Mathf.Cos(phi);
                float y = Mathf.Sin(theta) * Mathf.Sin(phi);
                float z = Mathf.Cos(theta);

                positions[i] = new Vector3(x, y, z)*ShapeSize;
            }

            return positions;
        }
    }
}