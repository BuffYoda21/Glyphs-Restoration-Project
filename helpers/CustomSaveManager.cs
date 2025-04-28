using System.Collections.Generic;
using System.IO;
using MelonLoader;
using MelonLoader.Utils;
using Newtonsoft.Json;
using UnityEngine;

namespace OldEndingRestoration.helpers {
    public static class CustomSaveManager {
        private static string SavePath => Path.Combine(MelonEnvironment.UserDataDirectory, "CustomSave.json");

        public class SlotData {
            public bool oldEndingSave1 = false;
            public bool oldEndingSave2 = false;
        }

        public class SaveData {
            public Dictionary<int, SlotData> Slots = new Dictionary<int, SlotData>();
        }

        private static SaveData currentSave = new SaveData();

        public static void Initialize() {
            if (!File.Exists(SavePath))
            {
                MelonLogger.Msg("Save file not found. Creating new save...");
                Save();
            }
            else
            {
                MelonLogger.Msg("Loading custom data...");
                Load();
            }
        }

        public static void Load() {
            try
            {
                string json = File.ReadAllText(SavePath);
                currentSave = JsonConvert.DeserializeObject<SaveData>(json) ?? new SaveData();
                MelonLogger.Msg("Custom data loaded successfully from slot " + GetCurrentSlot());
            }
            catch
            {
                MelonLogger.Error("Failed to load custom save file. Creating new save...");
                currentSave = new SaveData();
                Save();
            }
        }

        public static void Save() {
            try {
                string json = JsonConvert.SerializeObject(currentSave, Formatting.Indented);
                File.WriteAllText(SavePath, json);
                MelonLogger.Msg("Custom data saved to slot " + GetCurrentSlot());
            }
            catch {
                MelonLogger.Error("Failed to write custom save file");
            }
        }

        private static int GetCurrentSlot() {
            GameObject managerIntroObject = GameObject.Find("Manager intro");
            if (managerIntroObject != null) {
                SaveManager saveManager = managerIntroObject.GetComponent<SaveManager>();
                if (saveManager != null) {
                    return saveManager.currentslot;
                }
            }

            MelonLogger.Warning("Failed to determine current save slot. Defaulting to Slot 1.");
            return 1;
        }

        private static SlotData GetSlotData(int slot) {
            if (!currentSave.Slots.ContainsKey(slot)) {
                currentSave.Slots[slot] = new SlotData();
                Save();
            }
            return currentSave.Slots[slot];
        }

        public static bool GetOldEndingSave1() {
            int slot = GetCurrentSlot();
            return GetSlotData(slot).oldEndingSave1;
        }

        public static void SetOldEndingSave1(bool value) {
            int slot = GetCurrentSlot();
            GetSlotData(slot).oldEndingSave1 = value;
            Save();
        }
    }
}
