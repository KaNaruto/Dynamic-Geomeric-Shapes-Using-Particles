using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = UnityEngine.Random;

[BurstCompile]
public partial class SpawnParticleSystem : SystemBase
{
    private EntityQuery _spawnParticleConfigQuery;

    public delegate void ParticlesSpawnedHandler();
    public static event ParticlesSpawnedHandler OnParticlesAmountChanged;

    protected override void OnCreate()
    {
        _spawnParticleConfigQuery = GetEntityQuery(ComponentType.ReadOnly<SpawnParticleConfig>());
        RequireForUpdate(_spawnParticleConfigQuery);
    }

    protected override void OnStartRunning()
    {
        if (!SystemAPI.TryGetSingletonEntity<SpawnParticleConfig>(out Entity spawnParticleConfigEntity))
        {
            Debug.LogError("SpawnParticleConfig singleton not found. Ensure it is added to the world.");
            return;
        }

        SpawnParticleConfig spawnParticleConfig = EntityManager.GetComponentData<SpawnParticleConfig>(spawnParticleConfigEntity);
        SpawnParticles(spawnParticleConfig);
    }

    private void SpawnParticles(SpawnParticleConfig spawnParticleConfig)
    {
        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);

        for (int i = 0; i < spawnParticleConfig.AmountToSpawn; i++)
        {
            Entity spawnedEntity = entityCommandBuffer.Instantiate(spawnParticleConfig.ParticlePrefab);
            entityCommandBuffer.SetComponent(spawnedEntity, new LocalTransform
            {
                Position = new float3(Random.Range(-10f, +10f), Random.Range(-10f, +10f), Random.Range(-10f, +10f)),
                Rotation = quaternion.identity,
                Scale = 0.1f
            });
        }

        entityCommandBuffer.Playback(EntityManager);
        entityCommandBuffer.Dispose();

        OnParticlesAmountChanged?.Invoke();

        EntityQuery particleQuery = GetEntityQuery(typeof(Particle));
        NativeArray<Entity> particles = particleQuery.ToEntityArray(Allocator.TempJob);
        particles.Dispose();
    }

    private void DestroyParticles()
    {
        if (!SystemAPI.TryGetSingletonEntity<SpawnParticleConfig>(out Entity spawnParticleConfigEntity))
        {
            Debug.LogError("SpawnParticleConfig singleton not found. Ensure it is added to the world.");
            return;
        }

        SpawnParticleConfig spawnParticleConfig = EntityManager.GetComponentData<SpawnParticleConfig>(spawnParticleConfigEntity);

        EntityCommandBuffer entityCommandBuffer = new EntityCommandBuffer(Allocator.TempJob);
        EntityQuery particleQuery = GetEntityQuery(typeof(Particle));
        NativeArray<Entity> particles = particleQuery.ToEntityArray(Allocator.TempJob);

        int amountToDestroy = math.min(spawnParticleConfig.AmountToDestroy, particles.Length);
        for (int i = 0; i < amountToDestroy; i++)
        {
            entityCommandBuffer.DestroyEntity(particles[i]);
        }

        particles.Dispose();
        entityCommandBuffer.Playback(EntityManager);
        entityCommandBuffer.Dispose();
        OnParticlesAmountChanged?.Invoke();
    }

    public void UpdateParticleCount(int amount, float particleSize)
    {
        if (!SystemAPI.TryGetSingletonEntity<SpawnParticleConfig>(out Entity spawnParticleConfigEntity))
        {
            Debug.LogError("SpawnParticleConfig singleton not found. Ensure it is added to the world.");
            return;
        }

        SpawnParticleConfig spawnParticleConfig = EntityManager.GetComponentData<SpawnParticleConfig>(spawnParticleConfigEntity);
        if (amount < spawnParticleConfig.TotalAmount)
        {
            spawnParticleConfig.AmountToDestroy = spawnParticleConfig.TotalAmount - amount;
            spawnParticleConfig.TotalAmount = amount;
            EntityManager.SetComponentData(spawnParticleConfigEntity, spawnParticleConfig);
            DestroyParticles();
        }
        else
        {
            spawnParticleConfig.AmountToSpawn = amount - spawnParticleConfig.TotalAmount;
            spawnParticleConfig.TotalAmount = amount;
            EntityManager.SetComponentData(spawnParticleConfigEntity, spawnParticleConfig);
            SpawnParticles(spawnParticleConfig);
            UpdateParticleSize(particleSize); // Update size on spawn
        }
    }

    private void UpdateParticleSize(float size)
    {
        foreach (var localTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Particle>())
        {
            localTransform.ValueRW.Scale = size;
        }
    }

    protected override void OnUpdate()
    {
    }
}
