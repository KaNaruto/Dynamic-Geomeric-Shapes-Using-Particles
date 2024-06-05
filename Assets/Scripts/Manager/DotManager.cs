using System;
using System.Collections;
using System.Collections.Generic;
using Shapes;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class DotManager : MonoBehaviour
{
    [SerializeField] public float dotSize = 1.0f;
    [SerializeField] public int dotAmount = 100;
    [SerializeField] public float spacing = 0.2f;
    [SerializeField] private float moveSpeed = 1f;

    private const float YOffset = 5;

    private enum Shapes
    {
        Sphere,
        Torus,
        KleinBottleFigure8,
        KleinBottle,
        Lemniscate,
        MobiusStrip,
        TrefoilKnot,
        TorusKnot,
        VesicaPiscis,
        Squircle,
    }

    [SerializeField] private Shapes shape;
    [Range(1, 100)] [SerializeField] public int shapeSize = 1;

    private int _previousShapeSize;
    private int _previousDotAmount;
    private float _previousDotSize;
    private Shapes _previousShape;
    private float _previousMoveSpeed;
    
    private DotParticleSystem _dotParticleSystem;
    private SpawnParticleSystem _spawnParticleSystem;

    private static readonly Dictionary<Shapes, Type> ShapeTypeMap = new Dictionary<Shapes, Type>
    {
        { Shapes.Sphere, typeof(Sphere) },
        { Shapes.Torus, typeof(Torus) },
        { Shapes.KleinBottleFigure8, typeof(KleinBottleFigure8) },
        { Shapes.KleinBottle, typeof(KleinBottle) },
        { Shapes.Lemniscate, typeof(Lemniscate) },
        { Shapes.MobiusStrip, typeof(MobiusStrip) },
        { Shapes.TrefoilKnot, typeof(TrefoilKnot) },
        { Shapes.TorusKnot, typeof(TorusKnot) },
        { Shapes.VesicaPiscis, typeof(VesicaPiscis) },
        { Shapes.Squircle, typeof(Squircle) },
    };

    void OnEnable()
    {
        SpawnParticleSystem.OnParticlesAmountChanged += RecalculatePositions;
        SpawnParticleSystem.OnParticlesAmountChanged += UpdateSpeed;
    }

    void OnDisable()
    {
        SpawnParticleSystem.OnParticlesAmountChanged -= RecalculatePositions;
        SpawnParticleSystem.OnParticlesAmountChanged -= UpdateSpeed;
    }

    void Start()
    {
        _dotParticleSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<DotParticleSystem>();
        _spawnParticleSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<SpawnParticleSystem>();
        StartCoroutine(_dotParticleSystem.AllParticlesArrived());
        UpdatePreviousValues();
        StartCoroutine(StartFix());
    }

    void RecalculatePositions()
    {
        GeometricObject previousShape = gameObject.GetComponent<GeometricObject>();
        if (previousShape != null)
        {
            Destroy(previousShape);
        }

        if (ShapeTypeMap.TryGetValue(shape, out var shapeType))
        {
            var selectedShape = (GeometricObject)gameObject.AddComponent(shapeType);
            float3[] positions = selectedShape.GetPoints();
            if (positions != null)
            {
                positions = CenterPoints(positions);
                if (_dotParticleSystem != null && positions.Length == dotAmount)
                {
                    StopCoroutine(_dotParticleSystem.AllParticlesArrived());
                    StartCoroutine(_dotParticleSystem.AllParticlesArrived());
                    _dotParticleSystem.UpdateTargetPosition(positions);
                }
            }
        }
    }

    IEnumerator StartFix()
    {
        yield return new WaitForSeconds(2);
        shapeSize = 1;
        RecalculatePositions();
    }
 
    float3[] CenterPoints(float3[] positions)
    {
        float3 centroid = float3.zero;
        float3 upDirection = new float3(0, 1, 0);
        foreach (float3 pos in positions)
        {
            centroid += pos;
        }

        centroid /= positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= centroid - (upDirection * YOffset);
        }

        return positions;
    }

    private void Update()
    {
        CheckValues();
    }

    private void CheckValues()
    {
        if (shape != _previousShape)
        {
            RecalculatePositions();
            UpdatePreviousValues();
        }
        else if (dotAmount != _previousDotAmount)
        {
            UpdateParticleCount();
        }
        else if (dotSize != _previousDotSize)
        {
            UpdateDotSize();
        }
        else if (moveSpeed != _previousMoveSpeed)
        {
            UpdateSpeed();
        }
        else if (_previousShapeSize != shapeSize)
        {
            UpdateShapeSize();
        }
    }

    private void UpdateSpeed()
    {
        _dotParticleSystem.UpdateSpeed(moveSpeed);
        UpdatePreviousValues();
    }

    private void UpdateParticleCount()
    {
        _spawnParticleSystem.UpdateParticleCount(dotAmount, dotSize);
        UpdatePreviousValues();
    }

    private void UpdateShapeSize()
    {
        RecalculatePositions();
        UpdatePreviousValues();
    }

    private void UpdateDotSize()
    {
        _dotParticleSystem.UpdateSize(dotSize);
        UpdatePreviousValues();
    }

    void UpdatePreviousValues()
    {
        _previousShape = shape;
        _previousDotAmount = dotAmount;
        _previousDotSize = dotSize;
        _previousShapeSize = shapeSize;
        _previousMoveSpeed = moveSpeed;
    }
}
