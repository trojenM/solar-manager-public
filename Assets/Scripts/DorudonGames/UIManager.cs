using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace DorudonGames
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public GameObject upgradeTutorialPanel;
        [SerializeField] private Color baseCol;
        [SerializeField] private Color fadeCol;
        [SerializeField] private GameObject mergeButton;
        [SerializeField] private TextMeshProUGUI[] inc;
        private int cur = 0;
        private int max = 0;

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

            max = inc.Length;
            
        }

        public void CallText(float w, Vector3 targetPos)
        {
            cur++;
            if (cur >= max)
                cur = 0;
            var tex = inc[cur];
            tex.DORewind();
            tex.DOColor(baseCol,0f);
            tex.transform.DOMove(Camera.main.WorldToScreenPoint(targetPos), 0f);
            tex.transform.DOMove(Camera.main.WorldToScreenPoint(targetPos+Vector3.up), 0.75f);
            tex.text = w.ToString("C0",CultureInfo.CreateSpecificCulture("en-US"));
            tex.DOColor(fadeCol, 0.75f).OnComplete(()=>tex.alpha=0f);
        }
        public void EnableMergeButton() => mergeButton.SetActive(true);
    
        public void DisableMergeButton() => mergeButton.SetActive(false);

        public void EnableUpgradeTutorial() => upgradeTutorialPanel.SetActive(true);
        
        public void DisableUpgradeTutorial() => upgradeTutorialPanel.SetActive(false);
    }
}
