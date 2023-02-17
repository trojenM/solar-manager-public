using System;
using UnityEngine;

namespace DorudonGames
{
    public class PartControl : MonoBehaviour
    {
        [SerializeField] private float setCooldown = 2f;
        private ParticleSystem thisPart;
        private float cd = 0f;
        private Vector3 baseScale;
        void Awake()
        {
            cd = setCooldown;
            thisPart = GetComponent<ParticleSystem>();
            baseScale = transform.localScale;
        }
        
        
        void Update()
        {
            if (cd >= 0f)
            {
                cd -= Time.deltaTime;
            
            }else if (cd < 0)
            {
                cd = setCooldown;
                PartPool.Instance.AddToPool(thisPart);   
            }
        }

        private void OnDisable()
        {
            transform.localScale = baseScale;
        }
    }
}
