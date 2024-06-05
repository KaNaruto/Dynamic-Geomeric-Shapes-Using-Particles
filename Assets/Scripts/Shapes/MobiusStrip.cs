using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shapes
{
    public class MobiusStrip : GeometricObject
    {
        public override float3[] GetPoints()
        {
            float3[] positions = new float3[DotAmount];
            float width = 1.0f;

            for (int i = 0; i < DotAmount; i++)
            {
                float u = 2 * Mathf.PI * HaltonSequence(i,2); 
                float v = width * (Random.value * 2 - 1); 

                float cosU = Mathf.Cos(u);
                float sinU = Mathf.Sin(u);
                float halfU = u / 2;
                float cosHalfU = Mathf.Cos(halfU);
                float sinHalfU = Mathf.Sin(halfU);

                float x = (1 + v * cosHalfU) * cosU;
                float y = (1 + v * cosHalfU) * sinU;
                float z = v * sinHalfU;
            
                positions[i] = new Vector3(x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}