using System.Collections.Generic;
using UnityEngine;

namespace DorudonGames
{
    public class ObjectPool : MonoBehaviour
    {
        public string poolname;
        public GameObject prefab;
        private Queue<Planet> availableObjects = new Queue<Planet>();

        private void Awake()
        {
            GrowPool(10);
        }

        public Planet GetFromPool()
        {
            if (availableObjects.Count == 0)
            {
                GrowPool(1);   
            }

            var instance = availableObjects.Dequeue();
            instance.gameObject.SetActive(true);
            return instance.GetComponent<Planet>();
        }
    
        private void GrowPool(int n)
        {
            for (int i = 0; i < n; i++)
            {
                var instantiatedObj = Instantiate(prefab,transform);
                AddToPool(instantiatedObj.GetComponent<Planet>());
            }
        }

        public void AddToPool(Planet instance)
        {
            instance.gameObject.SetActive(false);
            availableObjects.Enqueue(instance);
            instance.transform.SetParent(transform);
        }
    }
}
