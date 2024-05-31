using UnityEngine;

public class Torus : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        float MajorRadius = 1.0f; 
        float MinorRadius = 0.5f; 
        Vector3[] positions = new Vector3[DotAmount];
        for (int i = 0; i < DotAmount; i++)
        {
            float u = 2 * Mathf.PI * HaltonSequence(i, 2);
            float v = 2 * Mathf.PI * HaltonSequence(i, 3);

            float x = (MajorRadius + MinorRadius * Mathf.Cos(v)) * Mathf.Cos(u);
            float y = (MajorRadius + MinorRadius * Mathf.Cos(v)) * Mathf.Sin(u);
            float z = MinorRadius * Mathf.Sin(v);

            positions[i] = new Vector3(x, y, z) * ShapeSize;
        }

        return positions;
    }
}