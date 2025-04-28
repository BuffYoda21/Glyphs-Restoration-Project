using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Collections;

namespace OldEndingRestoration.helpers {
    [HarmonyPatch(typeof(Pickup), "OnTriggerStay2D")]
    public static class Pickup_Patch {
        [HarmonyPostfix]
        static void Postfix(Collider2D other, Pickup __instance) {
            if (other.gameObject.name != "Player")
                return;
                
            //MelonLogger.Msg($"Pickup detected: {__instance.name}");

            GameObject player = GameObject.Find("Player");
            if(player != null) {
                PlayerController playerController = player.GetComponent<PlayerController>();
                if(playerController != null) {
                    MelonCoroutines.Start(DelayedCheck(playerController));
                }
                else {
                    MelonLogger.Warning("Failed to find PlayerController component on /Player");
                }
            }
            else {
                MelonLogger.Warning("Failed to find Player at /Player");
            }
        }

        static IEnumerator DelayedCheck(PlayerController playerController) {
            yield return new WaitForSeconds(15f);
            if (playerController.maxHp >= 150) {
                CustomSaveManager.SetOldEndingSave1(true);
                CustomSaveManager.Save();
                GameObject save1 = GameObject.Find("/World/Ending (old)/End1/Save Button/TPIndicator");   
                if(save1 != null) {
                    FastTravelUnlocker.Unlock(save1);
                    GameObject.Find("/MapManager/Tile 29,25").SetActive(false);
                }
                else {
                    MelonLogger.Warning("Failed to find /World/Ending (old)/End1/Save Button/TPIndicator");
                }
            }
        }
    }
}