using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DorudonGames
{
    public class PlanetPool : MonoBehaviour
    {
        public static PlanetPool Instance { get; private set; }
        
        public ObjectPool [] pools = new ObjectPool[6];

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

        }
        
    }
}
