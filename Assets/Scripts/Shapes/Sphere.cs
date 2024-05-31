using UnityEngine;

public class Sphere : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        Vector3[] positions = new Vector3[DotAmount];

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