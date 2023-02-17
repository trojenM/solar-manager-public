using Lofelt.NiceVibrations;

namespace DorudonGames
{
    public static class Vibrator
    {
        public static bool IsHapticOn = true;
    
        public static void HapticLight()
        {
            if(!IsHapticOn)
                return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
        
        public static void HapticMedium()
        {
            if(!IsHapticOn)
                return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
        
        public static void HapticHeavy()
        {
            if(!IsHapticOn)
                return;
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
        }

        public static void Vibrate()
        {
            if(!IsHapticOn)
                return;
        }

        public static void ToggleOnOff(bool b)
        {
            IsHapticOn = b;
        }
    }
}
