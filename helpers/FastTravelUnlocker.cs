using MelonLoader;
using UnityEngine;
namespace OldEndingRestoration.helpers
{
    public static class FastTravelUnlocker {
        public static void Unlock(GameObject tpIndicator) {
            if (tpIndicator == null) {
                MelonLogger.Warning("Err: TPIndicator cannot be null");
                return;
            }

            MapPin mapPin = tpIndicator.GetComponent<MapPin>();
            if (mapPin == null) {
                MelonLogger.Warning("Failed to find MapPin component on TPIndicator");
                return;
            }

            string appearCondition = mapPin.appearCondition;

            if (string.IsNullOrEmpty(appearCondition)) {
                MelonLogger.Warning("AppearCondition is empty. Nothing to unlock.");
                return;
            }

            GameObject managerIntroObject = GameObject.Find("Manager intro");
            if (managerIntroObject != null) {
                SaveManager saveManager = managerIntroObject.GetComponent<SaveManager>();

                if (saveManager != null) {
                    int currentSlot = saveManager.currentslot;
                    string key = $"Save{currentSlot}-{appearCondition}";
                    PlayerPrefs.SetString(key, "true");
                    tpIndicator.SetActive(true);
                }
                else {
                    MelonLogger.Warning("SaveManager component not found on Manager intro");
                }
            }
            else {
                MelonLogger.Warning("Failed to find /Manager intro");
            }
            mapPin.UpdateDisplay();
        }
    }
}
