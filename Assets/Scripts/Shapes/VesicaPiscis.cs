using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Shapes
{
    public class VesicaPiscis : GeometricObject
    {
        public  override float3[] GetPoints()
        {
            float3[] positions = new float3[DotAmount];
            float radius = 1.0f;
            float offset = radius; 

            for (int i = 0; i < DotAmount; i++)
            {
                float angle = 2 * Mathf.PI * HaltonSequence(i, 2);

           
                float x1 = radius * Mathf.Cos(angle);
                float y1 = radius * Mathf.Sin(angle);

            
                float x2 = x1 + offset;
                float y2 = y1;

           
                if (Random.value > 0.5f)
                {
                    positions[i] = new Vector3(x1, y1, 0) * ShapeSize;
                }
                else
                {
                    positions[i] = new Vector3(x2, y2, 0) * ShapeSize;
                }
            }

            return positions;
        }
    }
}