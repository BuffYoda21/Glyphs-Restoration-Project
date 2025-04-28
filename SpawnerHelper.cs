using MelonLoader;
using UnityEngine;
using HarmonyLib;

namespace OldEndingRestoration.Helpers {
    public static class SpawnerHelper {
        public static GameObject CloneAndPlace(string originalPath, string newName, string parentPath, Vector3 position) {
            GameObject original = GameObject.Find(originalPath);
            if (original == null) {
                MelonLogger.Warning($"Could not find original at {originalPath}");
                return null;
            }

            GameObject clone = UnityEngine.Object.Instantiate(original);
            clone.name = newName;
            clone.transform.position = position;

            if (!string.IsNullOrEmpty(parentPath)) {
                GameObject parent = GameObject.Find(parentPath);
                if (parent != null) {
                    clone.transform.SetParent(parent.transform);
                    clone.transform.localPosition = parent.transform.InverseTransformPoint(position);
                }
                else {
                    MelonLogger.Warning($"Could not find parent at {parentPath}");
                }
            }

            MelonLogger.Msg($"Spawned {newName} at {position}");
            return clone;
        }
    }
}
