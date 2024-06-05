using Unity.Entities;
using UnityEngine;

public class SpawnParticleConfigAuthoring : MonoBehaviour
{
    public GameObject particlePrefab;
    private int _amountToSpawn;
    private int _amountToDestroy;
    private int _totalAmount;

    public class SpawnParticleConfigBaker : Baker<SpawnParticleConfigAuthoring>
    {
        public override void Bake(SpawnParticleConfigAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity,
                new SpawnParticleConfig
                {
                    ParticlePrefab = GetEntity(authoring.particlePrefab, TransformUsageFlags.Dynamic),
                    AmountToSpawn = authoring._amountToSpawn,
                    AmountToDestroy = authoring._amountToDestroy,
                    TotalAmount = authoring._totalAmount
                });
        }
    }
}

public struct SpawnParticleConfig : IComponentData
{
    public Entity ParticlePrefab;
    public int TotalAmount;
    public int AmountToSpawn;
    public int AmountToDestroy;
}