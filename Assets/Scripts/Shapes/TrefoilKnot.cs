using UnityEngine;

public class TrefoilKnot : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        Vector3[] positions = new Vector3[DotAmount];
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
