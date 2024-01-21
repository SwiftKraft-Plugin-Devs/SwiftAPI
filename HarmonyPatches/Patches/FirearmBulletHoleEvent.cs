using HarmonyLib;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Modules;
using UnityEngine;

namespace SwiftAPI.HarmonyPatches.Patches
{
    public delegate void OnFirearmBulletHolePlaced(Vector3 pos, Firearm firearm);

    [HarmonyPatch(typeof(StandardHitregBase), nameof(StandardHitregBase.PlaceBulletholeDecal))]
    public class FirearmBulletHoleEvent
    {
        public static OnFirearmBulletHolePlaced Event;

        [HarmonyPostfix]
        public static void Postfix(RaycastHit hit, StandardHitregBase __instance)
        {
            Event?.Invoke(hit.point, __instance.Firearm);
        }
    }
}
