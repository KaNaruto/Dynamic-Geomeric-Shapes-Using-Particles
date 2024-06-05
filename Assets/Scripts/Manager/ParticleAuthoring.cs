using Unity.Entities;
using UnityEngine;

public class ParticleAuthoring : MonoBehaviour
{
    private float _speed = 1;
    private Vector3 _targetPosition;

    private class Baker : Baker<ParticleAuthoring>
    {
        public override void Bake(ParticleAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new TargetPosition { targetPosition = authoring._targetPosition });
            AddComponent(entity, new Particle());
            AddComponent(entity, new ParticleSpeed { Speed = authoring._speed });
        }
    }
}