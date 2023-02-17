using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DorudonGames
{
    
    public class DorudonUtils : MonoBehaviour
    {
        private static DorudonUtils _instance;
        
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize()
        {
            var obj = new GameObject
            {
                name = "DorudonUtils",
                hideFlags = HideFlags.HideInHierarchy,
                layer = 0,
                isStatic = true,
            };
            obj.AddComponent<DorudonUtils>();
        }
        
        private void Start()
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }

        #region Methods

        public static int GetLayerMask(string layerName, int existingMask = 0)
        {
            int layer = LayerMask.NameToLayer(layerName);
            return existingMask | (1 << layer);
        }

        #endregion
        
        #region Coroutine Call Methods

        public static void RunDelayedMethod(Action method, float delay)
        {
            _instance.StartCoroutine(RunDelayedMethodRoutine(method, delay));
        }
        
        
        public static void RunMethodAndDelay(Action methodFirst,Action methodLast, float delay)
        {
            _instance.StartCoroutine(RunMethodAndDelayRoutine(methodFirst,methodLast, delay));
        }

        public static void FillImageOverTime(Image img, float fillTarget, float targetTime)
        {
            _instance.StartCoroutine(FillImageOverTimeRoutine(img, fillTarget, targetTime));
        }

        public static void MoveToTargetAndBack( Transform moveTransform, Vector3 targetPos, float targetTime)
        {
            _instance.StartCoroutine(MoveToTargetAndBackRoutine(moveTransform, targetPos, targetTime));
            
        }
        
        #endregion

        #region Coroutines

        private static IEnumerator RunDelayedMethodRoutine(Action method, float delay)
        {
            yield return new WaitForSeconds(delay);
            method();
        }
        
        private static IEnumerator RunMethodAndDelayRoutine(Action methodFirst,Action methodLast, float delay)
        {
            methodFirst();
            yield return new WaitForSeconds(delay);
            methodLast();
        }

        private static IEnumerator FillImageOverTimeRoutine(Image img,float fillTarget,float targetTime)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            float curTime = 0f;
            while (curTime<targetTime)
            {
                curTime += Time.deltaTime;
                img.fillAmount = Mathf.MoveTowards(img.fillAmount, fillTarget, Time.deltaTime / targetTime);
                yield return waitForEndOfFrame;
            }

            img.fillAmount = fillTarget;
        }
        
        private static IEnumerator MoveToTargetAndBackRoutine(Transform moveTransform, Vector3 targetPos, float targetTime)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            Vector3 startPos = moveTransform.position;
            var dist = Vector3.Distance(startPos, targetPos);
            float curTime = 0f;
            while (curTime<targetTime)
            {
                curTime += Time.deltaTime;
                if (curTime < targetTime / 2)
                {
                    moveTransform.position = Vector3.MoveTowards(moveTransform.position, targetPos, (dist / targetTime)*Time.deltaTime*2f);    
                }
                else
                {
                    moveTransform.position = Vector3.MoveTowards(moveTransform.position, startPos, (dist / targetTime)*Time.deltaTime*2f);
                }
                
                yield return waitForEndOfFrame;
            }
        }
        

        #endregion
    }    
}
