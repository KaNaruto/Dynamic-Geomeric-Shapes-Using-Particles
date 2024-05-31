using UnityEngine;

public class KleinBottleFigure8 : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        float radius = 3.0f;  
        Vector3[] positions = new Vector3[DotAmount];
        for (int i = 0; i < DotAmount; i++)
        {
            float u = 2 * Mathf.PI * HaltonSequence(i, 2);
            float v = 2 * Mathf.PI * HaltonSequence(i, 3);

            float x = (radius + Mathf.Cos(u / 2) * Mathf.Sin(v) - Mathf.Sin(u / 2) * Mathf.Sin(2 * v)) * Mathf.Cos(u);
            float y = (radius + Mathf.Cos(u / 2) * Mathf.Sin(v) - Mathf.Sin(u / 2) * Mathf.Sin(2 * v)) * Mathf.Sin(u);
            float z = Mathf.Sin(u / 2) * Mathf.Sin(v) + Mathf.Cos(u / 2) * Mathf.Sin(2 * v);

            positions[i] = new Vector3(x, y, z) * ShapeSize;
        }

        return positions;
    }
}