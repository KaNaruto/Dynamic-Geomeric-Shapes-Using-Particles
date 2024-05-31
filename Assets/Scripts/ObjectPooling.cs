using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject objectPrefab;
    public int initialPoolSize = 100;
    private Queue<GameObject> _pool;
    private GameObject _objectPool;

    void Awake()
    {
        _objectPool = new GameObject("Object Pool");
        _pool = new Queue<GameObject>();
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject obj = Instantiate(objectPrefab, _objectPool.transform, true);
            obj.SetActive(false);
            _pool.Enqueue(obj);
        }
    }

    public GameObject GetObject()
    {
        if (_pool.Count > 0)
        {
            GameObject obj = _pool.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject obj = Instantiate(objectPrefab);
            return obj;
        }
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        _pool.Enqueue(obj);
    }
}