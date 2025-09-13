using HarmonyLib;
using UnityEngine;

// makes the character slowly become thirsty (about 2x the speed of hunger)

[HarmonyPatch(typeof(CharacterAfflictions), "UpdateNormalStatuses")]
public static class CharacterAfflictions_TargetMethod_Patch
{
    static void Postfix(CharacterAfflictions __instance)
    {
        if (!__instance.character.data.fullyConscious)
            return;

        CharacterAfflictions afflictions = __instance.character.refs.afflictions;
        float heatMultiplier = 1.0f + afflictions.GetCurrentStatus(CharacterAfflictions.STATUSTYPE.Hot) * 20;
        float delta = Time.deltaTime * __instance.hungerPerSecond * Ascents.hungerRateMultiplier * 2f * heatMultiplier;
        afflictions.AddStatus(ThirstAffliction.DehydrationType, delta);
    }
}
