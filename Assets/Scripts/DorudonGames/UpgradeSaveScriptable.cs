using UnityEngine;

namespace DorudonGames
{
    [CreateAssetMenu(fileName = "UpgradeData", menuName = "Scriptable/Create UpgradeData")]
    public class UpgradeSaveScriptable : ScriptableObject
    {
        public PenInfo penInfo;
        public UpgradeType[] upgradeTypes;
    }

    [System.Serializable]
    public class UpgradeType
    {
        public string upgradeName;
        public int upgradeLevel;
        public int[] upgradeCost;
        public float[] upgradeInfluence;
    }

    [System.Serializable]
    public class PenInfo
    {
        public float penLength;
        public float penIncome;
        public float penSpeed;
    }
}