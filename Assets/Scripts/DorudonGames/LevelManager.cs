using System;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DorudonGames
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance { get; private set; }
        
        [SerializeField] private GameObject guidelineObj,swipeTextObj,completeObj,failObj;
        [SerializeField] private TMP_Text coinText;
        [SerializeField] private Toggle hapticToggle;
        private UpgradeUI upgradeUI;
        private bool isFail = false;
	
        public float coin = 0;   
    

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

            upgradeUI = GameObject.FindObjectOfType<UpgradeUI>();
            DOTween.SetTweensCapacity(300,100);
        }

        void Start()
        {
            Application.targetFrameRate = 61;
            
            var hap = PlayerPrefs.GetInt("Haptic");
            if (hap == 0)
            {
                ToggleHaptics(true);
                hapticToggle.isOn = true;
            }
            else
            {
                ToggleHaptics(false);
                hapticToggle.isOn = false;
            }
            
            coin = PlayerPrefs.GetFloat("coin");
            LevelManager.Instance.UpdateMoney(0f);
        }

        public void ToggleHaptics(bool b)
        {
            // 0 = true , 1 = false
            if(b==true)
                PlayerPrefs.SetInt("Haptic",0);
            else
            {
                PlayerPrefs.SetInt("Haptic",1);
            }
            Vibrator.ToggleOnOff(b);
        }


        public void UpdateMoney(float amt)
        {
            coin += amt;
            coinText.text = ReturnNumber(coin);
            PlayerPrefs.SetFloat("coin",coin);
            upgradeUI.UpdateButtonInteractable();
        }
        
        string ReturnNumber(double number)
        {
            double bScore = number * 0.001;
            double mScore = number  * 0.000001 ;
            double kScore = number  * 0.000000001;

            if (number < 1000)
            {
                return number.ToString("C0", CultureInfo.CreateSpecificCulture("en-US"));
            }
            else if(number>=1000 && number<1000000)
            {
                return Math.Round(bScore,2).ToString( "C2", CultureInfo.CreateSpecificCulture("en-US"))+"K";
            }else if (number >= 1000000 && number<1000000000)
            {
                return Math.Round(mScore,2).ToString( "C2", CultureInfo.CreateSpecificCulture("en-US"))+"M";
            }
            else
            {
                return Math.Round(kScore,2).ToString( "C2", CultureInfo.CreateSpecificCulture("en-US"))+"B";
            }
 
        }
    
        public void LevelCompleted()
        {
            if(isFail)
                return;
            completeObj.SetActive(true);
        }
        public void LevelStart()
        {
            guidelineObj.SetActive(false);
        }
        public void LevelFailed()
        {
            isFail = true;
            failObj.SetActive(true);
        }
    
        public void RestartLevel()
        {
            DOTween.KillAll();
            int scene = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(scene);
        }
    
    
        public void NextLevel()
        {
            DOTween.KillAll();
            int idx = SceneManager.GetActiveScene().buildIndex + 1;
            if (idx <= 10)
                SceneManager.LoadScene(idx);
            else
            {
                idx = 1;
                SceneManager.LoadScene(idx);
            }
            
            PlayerPrefs.SetInt("levelIndex",idx);
        }
    }
}
