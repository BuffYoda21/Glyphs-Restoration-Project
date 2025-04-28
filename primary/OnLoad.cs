using MelonLoader;
using UnityEngine;
using OldEndingRestoration.helpers;
using System.Collections;

[assembly: MelonInfo(typeof(OldEndingRestoration.primary.OnLoad), "OldEndingRestoration", "0.4.0", "BuffYoda21")]
[assembly: MelonGame("J Daimond", "Glyphs")]
[assembly: MelonGame("Vortex Bros.", "GLYPHS")]

namespace OldEndingRestoration.primary {
    public class OnLoad : MelonMod {
        public static HarmonyLib.Harmony instance;
        
        public override void OnInitializeMelon() {
            instance = new HarmonyLib.Harmony("com.BuffYoda21.OldEndingRestoration");
            instance.PatchAll();
            MelonLogger.Msg("Harmony Initialized");
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName) {
            if (sceneName == "Game") {
                CustomSaveManager.Initialize();
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

                        if (CustomSaveManager.GetOldEndingSave1()) {
                            save1.SetActive(true);
                            MelonCoroutines.Start(DelayMapReveal());
                        }
                        else {
                            save1.SetActive(false);
                        }

                        GameObject.Find("/World/Ending (old)/End6/Save Button (2)").SetActive(false);
                        GameObject.Find("/World/Ending (old)/End6/Save Button (2)").name = "Save Button (old)";
                        SpawnerHelper.CloneAndPlace(
                            "/World/Region1/(R3A)/Save Button (HDD)",
                            "Save Button (new)",
                            "/World/Ending (old)/End6",
                            new Vector3(770.75f,-292.613f,10f)
                        ); //Save button placed here is active whenever the first save button on the map is. Intentionally left unfinished for now. Maybe I'll fix it later but for now I am tired.
                    }
                } else {
                    MelonLogger.Warning("Failed to find /World");
                }
            }
        }

        private static IEnumerator DelayMapReveal() {
            yield return new WaitForSeconds(0.2f);
            GameObject.Find("/MapManager/Tile 29,25").SetActive(false);
        }
    }
}
