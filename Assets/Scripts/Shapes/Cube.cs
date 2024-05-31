using UnityEngine;

public class Cube : GeometricObject
{
    public override Vector3[] GetPoints()
    {
        Vector3[] positions = new Vector3[DotAmount];
        int pointsPerFace = DotAmount / 6;
        
        for (int i = 0; i < DotAmount; i++)
        {
            float x = HaltonSequence(i, 2);
            float y = HaltonSequence(i, 3);
            float z = HaltonSequence(i, 5);
            
            int faceIndex = (i / pointsPerFace) % 6;
            
            switch (faceIndex)
            {
                case 0:
                    positions[i] = new Vector3(x, y, 0) * ShapeSize; // Front face
                    break;
                case 1:
                    positions[i] = new Vector3(x, y, 1) * ShapeSize; // Back face
                    break;
                case 2:
                    positions[i] = new Vector3(x, 0, z) * ShapeSize; // Bottom face
                    break;
                case 3:
                    positions[i] = new Vector3(x, 1, z) * ShapeSize; // Top face
                    break;
                case 4:
                    positions[i] = new Vector3(0, y, z) * ShapeSize; // Left face
                    break;
                case 5:
                    positions[i] = new Vector3(1, y, z) * ShapeSize; // Right face
                    break;
            }
        }

        return positions;
    }

    
}