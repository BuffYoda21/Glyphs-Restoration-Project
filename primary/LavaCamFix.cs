using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace OldEndingRestoration.primary {
    [HarmonyPatch(typeof(TriggerArea), "RunTrigger")]
    public static class TriggerArea_Patch {
        private static bool alreadyTriggered = false;

        [HarmonyPrefix]
        static bool Prefix(Collider2D other, TriggerArea __instance) {
            if (other.gameObject.name != "Player")
                return true;

            string objectPath = GetGameObjectPath(__instance.gameObject);

            if (objectPath == "/World/Ending (old)/EndBoss1/Trigger (1)") {
                if (!alreadyTriggered) {
                    alreadyTriggered = true;

                    GameObject world = GameObject.Find("World");
                    if (world != null) {
                        Transform endingOld = world.transform.Find("Ending (old)");
                        if (endingOld != null) {
                            Transform endBoss1 = endingOld.transform.Find("EndBoss1");
                            if (endBoss1 != null) {
                                Transform lava = endBoss1.transform.Find("Lava");
                                if (lava != null) {
                                    GameObject lavaObject = lava.gameObject;
                                    lavaObject.SetActive(true);
                                    MelonLogger.Msg("LavaRising started");
                                    MelonLogger.Msg("Cam fixed");

                                    __instance.gameObject.SetActive(false);
                                    return false;
                                } else {
                                    MelonLogger.Warning("Failed to find /World/Ending (old)/EndBoss1/Lava");
                                }
                            } else {
                                MelonLogger.Warning("Failed to find /World/Ending (old)/EndBoss1");
                            }
                        } else {
                            MelonLogger.Warning("Failed to find /World/Ending (old)");
                        }
                    } else {
                        MelonLogger.Warning("Failed to find /World");
                    }
                }

                return false;
            }

            return true;
        }

        static string GetGameObjectPath(GameObject obj) {
            string path = "/" + obj.name;
            while (obj.transform.parent != null) {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }
    }
}
