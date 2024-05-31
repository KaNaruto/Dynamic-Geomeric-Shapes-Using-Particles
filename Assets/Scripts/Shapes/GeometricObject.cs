using UnityEngine;

public abstract class GeometricObject : MonoBehaviour
{
    protected int DotAmount;
    protected int ShapeSize;
    protected float DotSize;
    protected float spacing;

    protected virtual void Awake()
    {
        DotAmount = FindObjectOfType<DotManager>().dotAmount;
        DotSize = FindObjectOfType<DotManager>().dotSize;
        spacing = FindObjectOfType<DotManager>().spacing;
        ShapeSize = FindObjectOfType<DotManager>().shapeSize;
    }

    public abstract Vector3[] GetPoints();
    
    protected float HaltonSequence(int index, int baseValue)
    {
        float result = 0f;
        float fraction = 1f / baseValue;
        while (index > 0)
        {
            result += (index % baseValue) * fraction;
            index /= baseValue;
            fraction /= baseValue;
        }
        return result;
    }
}