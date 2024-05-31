using UnityEngine;

public class Squircle : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        Vector3[] positions = new Vector3[DotAmount];
        
        for (int i = 0; i < DotAmount; i++)
        {
            float theta = 2 * Mathf.PI * HaltonSequence(i, 2);
            float phi = Mathf.Acos(2 * HaltonSequence(i, 3) - 1);


            float x = Mathf.Pow(Mathf.Abs(Mathf.Cos(theta)), 2.0f) * Mathf.Sign(Mathf.Cos(theta)) * Mathf.Sin(phi);
            float y = Mathf.Pow(Mathf.Abs(Mathf.Sin(theta)), 2.0f) * Mathf.Sign(Mathf.Sin(theta)) * Mathf.Sin(phi);
            float z = Mathf.Pow(Mathf.Abs(Mathf.Cos(phi)), 2.0f) * Mathf.Sign(Mathf.Cos(phi));

            positions[i] = new Vector3(x, y, z) * ShapeSize;
        }

        return positions;
    }
}