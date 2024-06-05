using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public class KleinBottle : GeometricObject
    {
        private const float TwoFifteenths = 2f / 15f;
        private const float OneFifteenth = 1f / 15f;
    
        public override float3[] GetPoints()
        {
            float3[] positions = new float3[DotAmount];
        
            for (int i = 0; i < DotAmount; i++)
            {
                float u = Mathf.PI * HaltonSequence(i, 2);
                float v = 2 * Mathf.PI * HaltonSequence(i, 3);

                float cosU = Mathf.Cos(u);
                float sinU = Mathf.Sin(u);
                float cosV = Mathf.Cos(v);
                float sinV = Mathf.Sin(v);
                float cosU2 = cosU * cosU;
                float cosU4 = cosU2 * cosU2;
                float cosU6 = cosU4 * cosU2;
                float cosU7 = cosU6 * cosU;
                float cosU5 = cosU6 * cosU;
                float cosU3 = cosU2 * cosU;
            

                float x = -TwoFifteenths * cosU * (3 * cosV - 30 * sinU + 90 * cosU4 * sinU - 60 * cosU6 * sinU + 5 * cosU * cosV * sinU);
                float y = -OneFifteenth * sinU * (3 * cosV - 3 * cosU2 * cosV - 48 * cosU4 * cosV + 48 * cosU6 * cosV - 60 * sinU + 
                    5 * cosU * cosV * sinU - 5 * cosU3 * cosV * sinU - 80 * cosU5 * cosV * sinU + 80 * cosU7 * cosV * sinU);
                float z = TwoFifteenths * (3 + 5 * cosU * sinU) * sinV;

                positions[i] = new Vector3(x, y, z) * ShapeSize;
            }

            return positions;
        }
    }
}