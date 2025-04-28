using MelonLoader;
using UnityEngine;
using HarmonyLib;
using OldEndingRestoration.Helpers;

[assembly: MelonInfo(typeof(OldEndingRestoration.OnLoad), "OldEndingRestoration", "0.3.0", "BuffYoda21")]
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
                bool restoreSuc = false;
                GameObject world = GameObject.Find("World");

                if (world != null) {
                    Transform endingOld = world.transform.Find("Ending (old)");
                    if (endingOld != null) {
                        endingOld.gameObject.SetActive(true);
                        endingOld.transform.position = new Vector3(684f, -351f, 0f);
                        MelonLogger.Msg("Restoration Successful!");
                        restoreSuc = true;
                    } else {
                        MelonLogger.Warning("Failed to find /World/Ending (old)");
                    }

                    if (restoreSuc) {
                        SpawnerHelper.CloneAndPlace(
                            "/World/Region1/(R3A)/Tiles/Square (1)",
                            "Floor",
                            "/World/Ending (old)/End1",
                            new Vector3(715f, -346.35f, 0f)
                        );

                        GameObject save1 = SpawnerHelper.CloneAndPlace(
                            "/World/Region1/(R3A)/Save Button (HDD)",
                            "Save Button",
                            "/World/Ending (old)/End1",
                            new Vector3(703f,-342f,0f)
                        ).transform.Find("TPIndicator").gameObject;
                        
                        if (true) { //logic check in .json file for if the button should be active
                            save1.SetActive(true);
                        } else {
                            save1.SetActive(false);
                        }
                    }
                } else {
                    MelonLogger.Warning("Failed to find /World");
                }
            }
        }
    }
}