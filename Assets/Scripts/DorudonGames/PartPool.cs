using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private Queue<ParticleSystem> availableObjects = new Queue<ParticleSystem>();
    public static PartPool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool(15);
    }

    public ParticleSystem GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool(5);   
        }

        var instance = availableObjects.Dequeue();
        return instance;
    }
    
    private void GrowPool(int n)
    {
        for (int i = 0; i < n; i++)
        {
            var instantiatedObj = Instantiate(prefab,transform).GetComponent<ParticleSystem>();
            AddToPool(instantiatedObj);
        }
    }

    public void AddToPool(ParticleSystem instance)
    {
        instance.transform.SetParent(transform);
        instance.gameObject.SetActive(false);
        availableObjects.Enqueue(instance);
        
    }
}
