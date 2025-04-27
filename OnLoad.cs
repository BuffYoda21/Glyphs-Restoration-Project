using MelonLoader;
using UnityEngine;
using HarmonyLib;

[assembly: MelonInfo(typeof(OldEndingRestoration.OnLoad), "OldEndingRestoration", "0.2.0", "BuffYoda21")]
[assembly: MelonGame("J Daimond", "Glyphs")]
[assembly: MelonGame("Vortex Bros.", "GLYPHS")]

namespace OldEndingRestoration {
    public class OnLoad : MelonMod {
        public static HarmonyLib.Harmony instance;
        
        public override void OnInitializeMelon() {
            instance = new HarmonyLib.Harmony("com.BuffYoda21.OldEndingRestoration");
            instance.PatchAll();
            MelonLogger.Msg("Harmony Initialized");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            if (sceneName == "Game") {
                MelonLogger.Msg("Restoring Regions...");
                GameObject world = GameObject.Find("World");
                if (world != null) {
                    Transform endingOld = world.transform.Find("Ending (old)");
                    if (endingOld != null) {
                        endingOld.gameObject.SetActive(true);
                        endingOld.transform.position = new Vector3(684f, -351f, 0f);
                        MelonLogger.Msg("Restoration Successful!");
                    }
                    else {
                        MelonLogger.Warning("Failed to find /World/Ending (old)");
                    }
                }
                else {
                    MelonLogger.Warning("Failed to find /World");
                }
            }
        }
    }
}