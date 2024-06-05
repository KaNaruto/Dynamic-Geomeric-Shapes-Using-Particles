using System.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
public partial class DotParticleSystem : SystemBase
{
    private bool _particlesOnPlace;

    protected override void OnUpdate()
    {
        if (!_particlesOnPlace)
        {
            var dotParticleJob = new DotParticleJob()
            {
                DeltaTime = SystemAPI.Time.DeltaTime
            };
            dotParticleJob.ScheduleParallel();
        }
    }

    public IEnumerator AllParticlesArrived()
    {
        while (true)
        {
            bool allParticlesArrived = true;
            Entities.ForEach((ref TargetPosition targetPosition) =>
            {
                if (!targetPosition.HasArrived)
                {
                    allParticlesArrived = false;
                }
            }).WithoutBurst().Run();

            if (allParticlesArrived)
            {
                _particlesOnPlace = true;
                yield break;
            }

            yield return new WaitForSeconds(1);
        }
    }

    public partial struct DotParticleJob : IJobEntity
    {
        public float DeltaTime;

        private void Execute(MoveParticleAspect moveParticleAspect)
        {
            moveParticleAspect.Move(DeltaTime);
        }
    }

    public void UpdateSize(float particleSize)
    {
        foreach (var localTransform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Particle>())
        {
            localTransform.ValueRW.Scale = particleSize;
        }
    }

    public void UpdateTargetPosition(float3[] positions)
    {
        int index = 0;
        foreach (var targetPosition in SystemAPI.Query<RefRW<TargetPosition>>().WithAll<Particle>())
        {
            if (index < positions.Length)
            {
                targetPosition.ValueRW.targetPosition = positions[index++];
            }
        }

        _particlesOnPlace = false;
        foreach (RefRW<TargetPosition> targetPosition in SystemAPI.Query<RefRW<TargetPosition>>().WithAll<Particle>())
        {
            targetPosition.ValueRW.HasArrived = false;
        }
    }

    public void UpdateSpeed(float speed)
    {
        foreach (var particleSpeed in SystemAPI.Query<RefRW<ParticleSpeed>>().WithAll<Particle>())
        {
            particleSpeed.ValueRW.Speed = speed;
        }
    }
}

public readonly partial struct MoveParticleAspect : IAspect
{
    private readonly RefRW<ParticleSpeed> _speed;
    private readonly RefRW<LocalTransform> _localTransform;
    private readonly RefRW<TargetPosition> _targetPosition;

    public void Move(float deltaTime)
    {
        float3 localPosition = _localTransform.ValueRO.Position;
        float3 targetPosition = _targetPosition.ValueRO.targetPosition;
        float distance = math.length(math.abs(localPosition - targetPosition));
        if (distance >= 0.01f)
        {
            Vector3 position = Vector3.MoveTowards(_localTransform.ValueRO.Position,
                _targetPosition.ValueRO.targetPosition, _speed.ValueRO.Speed * deltaTime);
            _localTransform.ValueRW.Position = position;
        }
        else
        {
            _targetPosition.ValueRW.HasArrived = true;
        }
    }
}

public struct ParticleSpeed : IComponentData
{
    public float Speed;
}

public struct TargetPosition : IComponentData
{
    public float3 targetPosition;
    public bool HasArrived;
}

public struct Particle : IComponentData
{
}
