using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DotManager : MonoBehaviour
{
    [SerializeField] GameObject dotPrefab;
    [SerializeField] public float dotSize = 1.0f; // Size of each particle
    [SerializeField] public int dotAmount = 100; // Total number of particles
    [SerializeField] public float spacing = 0.2f; // Spacing between particles
    [SerializeField] private float flowMagnitude = 0.1f; // Magnitude of the flow movement
    [SerializeField] private float flowSpeed = 1f; // Speed of the flow movement
    [SerializeField] private float moveSpeed = 1f; // Move speed of the particles

    private GameObject[] _dots;
    private Vector3[] _initialPositions;
    private const float yOffset = 5;

    GameObject _dotHolder;
    private ObjectPool objectPool;

    enum Shapes
    {
        Cube,
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
    [Range(1, 100)]
    [SerializeField] public int shapeSize;

    private int _previousShapeSize;
    private int _previousDotAmount;
    private float _previousDotSize;
    private float _previousSpacing;
    private Shapes _previousShape;

    private Coroutine _moveCoroutine; // Reference to the running coroutine

    void Start()
    {
        _dotHolder = new GameObject("Dot Holder");
        objectPool = GetComponent<ObjectPool>();

        if (objectPool == null)
        {
            Debug.LogError("ObjectPool component not found on the same GameObject.");
            return;
        }

        CreateDots(dotAmount);
        UpdatePreviousValues();
    }

    void CreateDots(int dotCount)
    {
        if (_dots == null)
        {
            _dots = new GameObject[dotCount];
            _initialPositions = new Vector3[dotCount];
            InstantiateDots(0, dotAmount);
        }

        int currentDotCount = _dots.Length;

        if (dotCount > currentDotCount)
        {
            int additionalDots = dotCount - currentDotCount;
            Array.Resize(ref _dots, dotCount);
            Array.Resize(ref _initialPositions, dotCount);

            InstantiateDots(currentDotCount, dotCount);
        }
        else if (dotCount < currentDotCount)
        {
            for (int i = dotCount; i < currentDotCount; i++)
            {
                objectPool.ReturnObject(_dots[i]);
            }

            Array.Resize(ref _dots, dotCount);
            Array.Resize(ref _initialPositions, dotCount);
        }

        RecalculatePositions();
    }

    void InstantiateDots(int startingPoint, int endPoint)
    {
        int[] spawnPositions = GetSpawnPoints(endPoint - startingPoint);
        int bestRows = spawnPositions[0];
        int bestColumns = spawnPositions[1];
        int bestDepth = spawnPositions[2];
        int index = startingPoint;

        for (int x = 0; x < bestRows; x++)
        {
            for (int y = 0; y < bestColumns; y++)
            {
                for (int z = 0; z < bestDepth; z++)
                {
                    Vector3 position = new Vector3(x * (dotSize + spacing), y * (dotSize + spacing), z * (dotSize + spacing)) + (Vector3.up * yOffset);
                    GameObject dot = objectPool.GetObject();
                    dot.transform.position = position;
                    dot.transform.localScale = Vector3.one * dotSize;
                    dot.transform.parent = _dotHolder.transform;
                    _dots[index] = dot;
                    _initialPositions[index] = position;
                    index++;
                }
            }
        }
    }

    int[] GetSpawnPoints(int amount)
    {
        int bestRows = 1;
        int bestColumns = 1;
        int bestDepth = amount;

        for (int rows = 1; rows <= amount; rows++)
        {
            for (int columns = 1; columns <= amount / rows; columns++)
            {
                int depth = amount / (rows * columns);

                if (rows * columns * depth == amount)
                {
                    if (Mathf.Abs(rows - columns) + Mathf.Abs(rows - depth) + Mathf.Abs(columns - depth) <
                        Mathf.Abs(bestRows - bestColumns) + Mathf.Abs(bestRows - bestDepth) +
                        Mathf.Abs(bestColumns - bestDepth))
                    {
                        bestRows = rows;
                        bestColumns = columns;
                        bestDepth = depth;
                    }
                }
            }
        }

        int[] temp = { bestRows, bestColumns, bestDepth };
        return temp;
    }

    void RecalculatePositions()
    {
        // Remove the previous shape component if it exists
        GeometricObject previousShape = gameObject.GetComponent<GeometricObject>();
        if(previousShape != null)
            Destroy(previousShape);

        GeometricObject selectedShape = null;
        switch (shape)
        {
            case Shapes.Cube:
                selectedShape = gameObject.AddComponent<Cube>();
                break;
            case Shapes.Sphere:
                selectedShape = gameObject.AddComponent<Sphere>();
                break;
            case Shapes.Torus:
                selectedShape = gameObject.AddComponent<Torus>();
                break;
            case Shapes.KleinBottleFigure8:
                selectedShape = gameObject.AddComponent<KleinBottleFigure8>();
                break;
            case Shapes.KleinBottle:
                selectedShape = gameObject.AddComponent<KleinBottle>();
                break;
            case Shapes.Lemniscate:
                selectedShape = gameObject.AddComponent<Lemniscate>();
                break;
            case Shapes.MobiusStrip:
                selectedShape = gameObject.AddComponent<MobiusStrip>();
                break;
            case Shapes.TrefoilKnot:
                selectedShape = gameObject.AddComponent<TrefoilKnot>();
                break;
            case Shapes.TorusKnot:
                selectedShape = gameObject.AddComponent<TorusKnot>();
                break;
            case Shapes.VesicaPiscis:
                selectedShape = gameObject.AddComponent<VesicaPiscis>();
                break;
            case Shapes.Squircle:
                selectedShape = gameObject.AddComponent<Squircle>();
                break;
        }

        if (selectedShape != null)
        {
            Vector3[] positions = selectedShape.GetPoints();
            positions = CenterPoints(positions);
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
            }
            _moveCoroutine = StartCoroutine(MoveToNewPosition(positions));
        }
    }

    Vector3[] CenterPoints(Vector3[] positions)
    {
        Vector3 centroid = Vector3.zero;
        foreach (Vector3 pos in positions)
        {
            centroid += pos;
        }
        centroid /= positions.Length;

        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] -= centroid-(Vector3.up*yOffset);
        }

        return positions;
    }

    private void Update()
    {
        if (shape != _previousShape)
        {
            RecalculatePositions();
            UpdatePreviousValues();
        }
        else if (dotAmount != _previousDotAmount)
        {
            CreateDots(dotAmount);
            UpdatePreviousValues();
        }
        else if (dotSize != _previousDotSize)
        {
            ResizeDots(dotSize);
            UpdatePreviousValues();
        }
        else if (_previousShapeSize != shapeSize)
        {
            RecalculatePositions();
            UpdatePreviousValues();
        }
    }

    void ResizeDots(float size)
    {
        foreach (GameObject dot in _dots)
            dot.transform.localScale = Vector3.one * size;
        
        RecalculatePositions();
    }

    void UpdatePreviousValues()
    {
        _previousShape = shape;
        _previousDotAmount = dotAmount;
        _previousDotSize = dotSize;
        _previousSpacing = spacing;
        _previousShapeSize = shapeSize;
    }

    IEnumerator MoveToNewPosition(Vector3[] positions)
    {
        bool everyDotOnPosition = false;
        while (!everyDotOnPosition)
        {
            everyDotOnPosition = true;
            for (int i = 0; i < positions.Length; i++)
            {
                _dots[i].transform.position = Vector3.MoveTowards(_dots[i].transform.position, positions[i],
                    moveSpeed * Time.deltaTime);
                if (Vector3.Distance(_dots[i].transform.position, positions[i]) > 0.01f)
                {
                    everyDotOnPosition = false;
                }
            }

            yield return null;
        }
    }

    void FixedUpdate()
    {
        if (false)
        {
            // Uncomment to enable flowing motion
            for (int i = 0; i < _dots.Length; i++)
            {
                Vector3 randomOffset = new Vector3(
                    Mathf.PerlinNoise(Time.time * flowSpeed + i, 0) - 0.5f,
                    Mathf.PerlinNoise(0, Time.time * flowSpeed + i) - 0.5f,
                    Mathf.PerlinNoise(Time.time * flowSpeed - i, Time.time * flowSpeed + i) - 0.5f
                ) * flowMagnitude;

                _dots[i].transform.position = _initialPositions[i] + randomOffset;
            }
        }
    }
}
