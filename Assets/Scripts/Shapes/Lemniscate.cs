using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public  class Lemniscate : GeometricObject
    {
        private const float A = 0.97f; 
        private const float B = 1f;
    
        public  override float3[] GetPoints()
        {
            float b4 = B * B * B * B;
            float a4 = A * A * A * A;
            float3[] positions = new float3[DotAmount];
            for (int i = 0; i < DotAmount; i++)
            {
                float u = 2 * Mathf.PI * HaltonSequence(i, 2);
                float v = Mathf.PI * HaltonSequence(i, 3);
                float m = (A * A) * Mathf.Cos(u * 2) + (Mathf.Sqrt(b4 - a4 + (a4 * (Mathf.Cos(2 * u) * Mathf.Cos(2 * u)))));

                float sqrtM = Mathf.Sqrt(m);
                float x = Mathf.Cos(u) * sqrtM;
                float y = Mathf.Sin(u) * sqrtM;
                float z = y * Mathf.Sin(v);
                y *= Mathf.Cos(v);
            
                positions[i] = new Vector3(-x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}