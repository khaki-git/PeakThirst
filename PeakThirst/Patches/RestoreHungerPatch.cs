using HarmonyLib;
using PeakThirst;
using System;
using UnityEngine;

[HarmonyPatch(typeof(Action_RestoreHunger), "RunAction")]
public static class Action_RestoreHunger_TargetMethod_Patch
{
    private static readonly Func<ItemAction, Character> GetCharacter =
        AccessTools.MethodDelegate<Func<ItemAction, Character>>(
            AccessTools.PropertyGetter(typeof(ItemAction), "character")
        );

    static void Postfix(Action_RestoreHunger __instance)
    {
        var character = GetCharacter(__instance);
        character.refs.afflictions.SubtractStatus(ThirstAffliction.DehydrationType, __instance.restorationAmount / 4);
    }
}
