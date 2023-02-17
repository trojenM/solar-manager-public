using System;
using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DorudonGames
{
    public class UpgradeUI : MonoBehaviour
    {
        [SerializeField] private OrbitalManager orbitalManager;
        [SerializeField] private Image[] innerImages;
        [SerializeField] private Sprite activeSprite, inactiveSprite;
        public TMP_Text sizeLvlTxt, incomeLvlTxt, planetLvlTxt;
        public TMP_Text sizeLevelCostTxt, incomeCostTxt, planetCostTxt;
        public Button sizeBtn, incomeBtn, planetBtn;
        public Transform sizeBtnTr, incomeBtnTr, planetBtnTr;
        private Camera cam;
        private Vector3 camStartPos;
        
        private float basePSize=250, basePIncome=35, basePPlanet=50;
        private float sizeP=20, incomeP=20, planetP=20;
        public static float incomeMul = 1.0f;
        private float buyMul = 1.0f;
        private int sizeLv = 1;
        private int maxSize = 6;

        private void Start()
        {
            cam = Camera.main;
            
            
            sizeLv = PlayerPrefs.GetInt("OrbitSize");
            
            var tempBasePSize = PlayerPrefs.GetFloat("basePSize");
            var tempBasePIncome = PlayerPrefs.GetFloat("basePIncome");
            var tempBasePPlanet = PlayerPrefs.GetFloat("basePPlanet");
            var tempBuyMul = PlayerPrefs.GetFloat("buyMul");
            
            if (tempBasePSize != 0)
            {
                basePSize = tempBasePSize;
            }
            
            if (tempBasePIncome != 0)
            {
                basePIncome = tempBasePIncome;
                incomeMul = PlayerPrefs.GetFloat("incomeMul");
            }
            
            if (tempBasePPlanet != 0)
            {
                basePPlanet = tempBasePPlanet;
            }

            if (tempBuyMul != 0)
            {
                buyMul = tempBuyMul;
            }
            
            if(sizeLv>2)
            {
                for (int i = 2; i <= sizeLv; i++)
                {
                    orbitalManager.UnlockNextOrbit();
                }
                
                orbitalManager.isTutorial = true; 
                
                
            }
            else
            {
                sizeLv = 1;
                sizeLv++;
                orbitalManager.UnlockNextOrbit();
            }
            
            RecalculatePrices();
            UpdateButtonInteractable();
            
            if (cam is not null)
            {
                camStartPos = cam.transform.position;
                cam.transform.DORewind();
                if(sizeLv>2)
                    cam.transform.DOMove(camStartPos+(-cam.transform.forward*sizeLv*2.333f),0.25f);
                else
                {
                    cam.transform.DOMove(camStartPos+(-cam.transform.forward*sizeLv*0.8f),0.25f);
                }
            }
        }

        public void RecalculatePrices()
        {
            if (sizeLv >= 3)
            {
                sizeP = Mathf.Floor(basePSize + (basePSize * buyMul * 6f));
            }else if (sizeP > 1_000_000_000)
            {
                sizeP = basePSize;
            }
            else
            {
                sizeP = Mathf.Floor(basePSize + (basePSize * buyMul * 2.4f));

            }
            
            if (basePIncome > 100000)
            {
                incomeP = Mathf.Floor(basePIncome + (basePIncome * buyMul * 0.005f));
            }else if (incomeP > 1_000_000_000)
            {
                incomeP = basePIncome;
            }
            else
            {
                incomeP = Mathf.Floor(basePIncome + (basePIncome * buyMul * 0.15f));
            }
            
            if (basePPlanet > 100000)
            {
                planetP = Mathf.Floor(basePPlanet + (basePPlanet * buyMul * 0.005f));
            }else if (basePPlanet > 1_000_000_000)
            {
                planetP = basePPlanet;
            }
            else
            {
                planetP = Mathf.Floor(basePPlanet + (basePPlanet * buyMul * 0.15f));
            }

            sizeLevelCostTxt.text = ReturnNumber(sizeP);
            incomeCostTxt.text = ReturnNumber(incomeP);
            planetCostTxt.text = ReturnNumber(planetP);

            if (sizeLv >= maxSize)
            {
                sizeLevelCostTxt.text = "MAX";
            }
                
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

        public void UpgradeEvent(int btnIndex)
        {
            switch (btnIndex)
            {
                case 0:
                    basePSize = sizeP;
                    PlayerPrefs.SetFloat("basePSize",basePSize);
                    PlayButtonAnimation(btnIndex);
                    buyMul += 0.02f;
                    PlayerPrefs.SetFloat("buyMul",buyMul);
                    sizeLv++;
                    if (sizeLv >= 2)
                        orbitalManager.isTutorial = true;
                    orbitalManager.UnlockNextOrbit();
                    PlayerPrefs.SetInt("OrbitSize",sizeLv);
                    cam.transform.DORewind();
                    cam.transform.DOMove(camStartPos+(-cam.transform.forward*sizeLv*2.333f),0.25f);
                    UIManager.Instance.DisableUpgradeTutorial();
                    LevelManager.Instance.UpdateMoney(-sizeP);
                    break;
                case 1:
                    basePIncome = incomeP;
                    PlayerPrefs.SetFloat("basePIncome",basePIncome);
                    incomeMul += 0.1f;
                    PlayerPrefs.SetFloat("incomeMul",incomeMul);
                    incomeMul = Convert.ToSingle(Math.Round(incomeMul, 2));
                    buyMul += 0.02f;
                    PlayerPrefs.SetFloat("buyMul",buyMul);
                    LevelManager.Instance.UpdateMoney(-incomeP);
                    PlayButtonAnimation(btnIndex);
                    break;
                case 2:
                    basePPlanet = planetP;
                    PlayerPrefs.SetFloat("basePPlanet",basePPlanet);
                    orbitalManager.SpawnNewPlanetAtIndex(0);
                    LevelManager.Instance.UpdateMoney(-planetP);
                    PlayButtonAnimation(btnIndex);
                    break;
            }
            
            RecalculatePrices();
        }

        public void UpdateButtonInteractable()
        {
            RecalculatePrices();

            sizeLvlTxt.text = sizeLv.ToString();
            incomeLvlTxt.text = incomeMul.ToString(CultureInfo.InvariantCulture)+"x";
            planetLvlTxt.text = "+1";

            if (sizeP < LevelManager.Instance.coin && sizeLv < maxSize)
            {
                sizeBtn.interactable = true;
                innerImages[0].sprite = activeSprite;
            }
            else
            {
                sizeBtn.interactable = false;
                innerImages[0].sprite = inactiveSprite;
            }

            if (incomeP < LevelManager.Instance.coin)
            {
                incomeBtn.interactable = true;
                innerImages[1].sprite = activeSprite;
            }
            else
            {
                incomeBtn.interactable = false;
                innerImages[1].sprite = inactiveSprite;
            }

            if (planetP < LevelManager.Instance.coin)
            {
                planetBtn.interactable = true;
                innerImages[2].sprite = activeSprite;
            }
            else
            {
                planetBtn.interactable = false;
                innerImages[2].sprite = inactiveSprite;
            }
        }

        private void PlayButtonAnimation(int btnIndex)
        {
            switch (btnIndex)
            {
                case 0:
                    sizeBtnTr.DORewind();
                    sizeBtnTr.DOPunchScale(Vector3.one * 0.2f, 0.5f, 0, 0f);
                    break;
                case 1:
                    incomeBtnTr.DORewind();
                    incomeBtnTr.DOPunchScale(Vector3.one * 0.2f, 0.5f, 0, 0f);
                    break;
                case 2:
                    planetBtnTr.DORewind();
                    planetBtnTr.DOPunchScale(Vector3.one * 0.2f, 0.5f, 0, 0f);
                    break;
            }
        }
    }
}
