using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Object Pooler per un singolo tipo di MonoBehaviour.<br/>
/// Se vengono inseriti MonoBehaviour di natura diversa, potrebbero esserci dei comportamenti inaspettati
/// </summary>
/// <typeparam name="T"></typeparam>
public class ObjectPooler<T> where T : MonoBehaviour
{
    private T monoBehaviourPrefab;
    private Queue<T> objectPool;

    public ObjectPooler(T prefab)
    {
        objectPool = new Queue<T>();
        monoBehaviourPrefab = prefab;
    }

    public T Get()
    {
        if (objectPool.Count > 0)
        {
            return objectPool.Dequeue();
        }

        if (monoBehaviourPrefab == null)
            throw new NullReferenceException("Non è stato inizializzato il monoBehaviourPrefab.");

        return GameObject.Instantiate(monoBehaviourPrefab);
    }

    public void Set(T monoBehaviour)
    {
        objectPool.Enqueue(monoBehaviour);
    }
}
