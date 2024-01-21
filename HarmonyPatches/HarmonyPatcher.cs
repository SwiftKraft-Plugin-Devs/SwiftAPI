using HarmonyLib;
using PluginAPI.Core;
using System;

namespace SwiftAPI.HarmonyPatches
{
    public static class HarmonyPatcher
    {
        public static Harmony Instance;

        public static void InitHarmony()
        {
            if (Instance == null)
                Instance = new Harmony("com.SwiftKraft.SwiftAPI");

            try { Instance.PatchAll(); }
            catch (Exception e) { Log.Error("Harmony Patching Failed! \n" + e?.ToString()); }
        }

        public static void DeinitHarmony()
        {
            Instance?.UnpatchAll();
        }
    }
}
