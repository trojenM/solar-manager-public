using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PathCreation;
using PathCreation.Examples;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DorudonGames
{
    public class OrbitalManager : MonoBehaviour
    {
        [SerializeField] private PathCreator [] paths = new PathCreator[6];
        public List<Planet>[] planets = new List<Planet>[6];
        public List<LineRenderer> lineRenderers = new List<LineRenderer>();
        public MoneyAdder[] moneyAdders;
        public float[] moneyAdderZScales;

        [SerializeField] private float planetsBaseSpeed;
        [SerializeField] private float planetsFullSpeed;
        private InputManager input;
        public int unlockedOrbits = 0;
        public static event Action<float> UpdatePlanetsSpeed;
        public bool isTutorial = false;

        void Awake()
        {
            
        }

        private void Start()
        {
            input = GetComponent<InputManager>();
            for (var i = 0; i < planets.Length; i++)
            {
                lineRenderers.Add(paths[i].GetComponent<LineRenderer>());
                planets[i]= new List<Planet>();
            }

            lineRenderers[0].enabled = true;

           
            
            for (int i = 0; i < planets.Length; i++)
            {
                var planetCount = PlayerPrefs.GetInt("planets" + i);
                for (int j = 0; j < planetCount; j++)
                {
                    SpawnNewPlanetAtIndex(i);
                }
            }
            CheckMerge();
            
            if (PlayerPrefs.GetInt("planets0") <= 0)
            {
                SpawnNewPlanetAtIndex(0);
                CheckMerge();  
            }
        }

        void Update()
        {
            UpdatePlanetsSpeed?.Invoke(planetsBaseSpeed + (input.value * planetsFullSpeed));
        }

        public void UnlockNextOrbit()
        {
            unlockedOrbits++;
            moneyAdders[unlockedOrbits].gameObject.SetActive(true);
            foreach (var ma in moneyAdders)
            {
                ma.SetScaleZ(moneyAdderZScales[unlockedOrbits]);
            }
            lineRenderers[unlockedOrbits].enabled = true;
            CheckMerge();
        }

        public void SpawnNewPlanetAtIndex(int idx)
        {
            
            
            var instance = PlanetPool.Instance.pools[idx].GetFromPool();
            var follower = instance.follower;
            follower.pathCreator = paths[idx];
            planets[idx].Add(instance);
            
            PlayerPrefs.SetInt("planets"+idx, planets[idx].Count);
            
            float startPos = Random.Range(0f, paths[idx].path.length);
            for (int i = 0; i < 10; i++)
            {
                if (planets[idx].Exists(x => Mathf.Abs(x.follower.distanceTravelled - startPos) < 35f))
                {
                    startPos = Random.Range(0f, paths[idx].path.length);
                    continue;
                }
                else
                {
                    
                    break;
                }
            }
            follower.distanceTravelled = startPos;
            instance.gameObject.SetActive(true);
            instance.tr.DOScale(instance.baseSize, 0.3f);
            var part = PartPool.Instance.GetFromPool();
            Transform tr;
            (tr = part.transform).SetParent(instance.tr);
            tr.localPosition = Vector3.zero;
            part.gameObject.SetActive(true);
            part.Play();
            CheckMerge();
        }

        public void MergePlanets()
        {
            Vibrator.HapticHeavy();
            for (int i = 0; i < unlockedOrbits; i++)
            {
                if (planets[i].Count >2)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        var p = planets[i][0];
                        planets[i].RemoveAt(0);
                        p.tr.DOScale(Vector3.one*0.0001f, 0.3f).OnComplete(() =>
                        {
                            PlanetPool.Instance.pools[i].AddToPool(p);    
                        });
                    }
                    PlayerPrefs.SetInt("planets"+i, planets[i].Count);
                    SpawnNewPlanetAtIndex(i+1);
                    CheckTutorial();
                    return;
                }
            }
        }

        private void CheckTutorial()
        {
            if (isTutorial)
            {
                return;
            }
                

            if (planets[1].Count >= 3)
            {
                UIManager.Instance.EnableUpgradeTutorial();
                isTutorial = true;
            }
        }

        public void CheckMerge()
        {
            for (int i = 0; i < unlockedOrbits; i++)
            {
                if (planets[i].Count >2)
                {
                    //Enable merge button
                    UIManager.Instance.EnableMergeButton();
                    return;
                }
            }

            //Disable merge button
            UIManager.Instance.DisableMergeButton();
            return;
        }
    }
}
