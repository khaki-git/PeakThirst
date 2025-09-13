using HarmonyLib;
using UnityEngine;

namespace PeakThirst.Patches
{
    [HarmonyPatch(typeof(StormVisual))]
    internal static class StormVisualRainHydrationPatch
    {
        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        private static void Postfix(StormVisual __instance)
        {
            if (__instance.stormType != StormVisual.StormType.Rain) return;
            if (!__instance.playerInWindZone) return;

            var c = Character.observedCharacter;
            if (c == null || !c.IsLocal) return;

            c.refs.afflictions.SubtractStatus(ThirstAffliction.DehydrationType, 0.1f * Time.deltaTime);
        }
    }
}
