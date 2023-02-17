using System;
using System.Collections.Generic;
using System.Globalization;
using DG.DemiEditor;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DorudonGames
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Color normalColor, fadedColor;
        [SerializeField] private Material trailMat;
        [SerializeField] private Animator incAnim;
        [SerializeField] private OrbitalManager orbitalManager;
        [SerializeField] private TMP_Text incText;
        [SerializeField] private int UILayer;
        [SerializeField] private int incSegment;
        [SerializeField] private float decInterval;
        [SerializeField] private float baseMul = 7f;
        [SerializeField] private float[] planetIncomes;
        private float mul;
        private bool isShowing = false;
        
        private float _interval;
        public float value;
        
        private void Awake()
        {
            _interval = decInterval;
            UILayer = LayerMask.NameToLayer("UI");
        }

        private void Start()
        {
            HideTrail();
            UpdateIncomeForSecond();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) && !IsPointerOverUIElement())
            {
                if (!isShowing)
                {
                    ShowTrail();
                    isShowing = true;
                }
                
                value += 1f / incSegment; 
                _interval = decInterval;
                UpdateIncomeForSecond();
                incAnim.SetTrigger("inc");
                Vibrator.HapticLight();
            }
            
            _interval -= Time.deltaTime;
            
            if (_interval <= 0f)
            {
                isShowing = false;
                HideTrail();
                value -= 1f / incSegment;
                _interval = decInterval;
                UpdateIncomeForSecond();
            }

            value = Mathf.Clamp01(value);
        }

        private void HideTrail()
        {
            trailMat.DOKill();
            trailMat.DOColor(fadedColor,0.4f);
        }
        
        private void ShowTrail()
        {
            trailMat.DOKill();
            trailMat.DOColor(normalColor,0.4f);
        }
        
        private void UpdateIncomeForSecond()
        {
            
            
            mul = baseMul - value * 4;
            float inc = 0;
            for (var i = 0; i < orbitalManager.planets.Length; i++)
            {
                var count = orbitalManager.planets[i].Count;
                inc += count * planetIncomes[i];
            }

            incText.text = ReturnNumber(Math.Round((inc / mul)*UpgradeUI.incomeMul*orbitalManager.unlockedOrbits))+ "/sec";
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
        
        public bool IsPointerOverUIElement()
        {
            return IsPointerOverUIElement(GetEventSystemRaycastResults());
        }
    
        private bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
        {
            for (int index = 0; index < eventSystemRaysastResults.Count; index++)
            {
                RaycastResult curRaysastResult = eventSystemRaysastResults[index];
                if (curRaysastResult.gameObject.layer == UILayer)
                    return true;
            }
            return false;
        }
        
        
        static List<RaycastResult> GetEventSystemRaycastResults()
        {
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = Input.mousePosition;
            List<RaycastResult> raysastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, raysastResults);
            return raysastResults;
        }
    }
}
