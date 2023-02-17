using UnityEngine;

namespace DorudonGames
{
    public class PlanetRotator : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed;
        private Transform _tr;

        private void Awake()
        {
            _tr = transform;
        }

        void FixedUpdate()
        {
            _tr.RotateAround(_tr.position, transform.up, Time.deltaTime * rotationSpeed);
        }
    }
}
