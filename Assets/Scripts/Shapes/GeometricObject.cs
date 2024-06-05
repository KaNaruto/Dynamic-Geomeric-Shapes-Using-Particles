using Unity.Mathematics;
using UnityEngine;

namespace Shapes
{
    public abstract class GeometricObject : MonoBehaviour
    {
        protected int DotAmount;
        protected int ShapeSize;
    

        protected virtual void Awake()
        {
            DotAmount = FindObjectOfType<DotManager>().dotAmount;
            ShapeSize = FindObjectOfType<DotManager>().shapeSize;
        }

        public abstract float3[] GetPoints();
    
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
}