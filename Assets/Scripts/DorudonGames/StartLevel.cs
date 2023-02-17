using UnityEngine;

namespace DorudonGames
{
    public class StartLevel : MonoBehaviour
    {
        private bool _isStarted = false;
    
        void Start()
        {
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0) && !_isStarted)
            {
                _isStarted = true;
                LevelManager.Instance.LevelStart();
                this.enabled = false;
            }        
        }
    }
}
