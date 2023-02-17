using System;
using DG.Tweening;
using PathCreation.Examples;
using UnityEngine;

namespace DorudonGames
{
    public class Planet : MonoBehaviour
    {
        public int moneyToAdd;
        public PathFollower follower;
        public Transform tr;
        public Vector3 baseSize;

        private void Awake()
        {
            tr = transform;
            follower = GetComponent<PathFollower>();
            baseSize = tr.localScale;
            tr.localScale = Vector3.one * 0.0001f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("ma"))
            {
                UIManager.Instance.CallText(moneyToAdd * UpgradeUI.incomeMul,tr.position);
                LevelManager.Instance.UpdateMoney(moneyToAdd * UpgradeUI.incomeMul);
            }
        }

        private void OnEnable()
        {
            OrbitalManager.UpdatePlanetsSpeed += UpdateSpeed;
        }

        private void OnDisable()
        {
            OrbitalManager.UpdatePlanetsSpeed -= UpdateSpeed;
        }

        private void UpdateSpeed(float value)
        {
            follower.speed = value;
        }
    }
}
