/*
 float additionalThirst = ThirstRestorationCalculator.CalculateThirstRestoration(__instance.gameObject);
 
 */

using HarmonyLib;
using PeakThirst;
using System;
using UnityEngine;

// drinky water

[HarmonyPatch(typeof(Item), "Awake")]
public static class Item_TargetMethod_Patch
{
    static void Postfix(Item __instance)
    {
        Debug.Log(__instance.gameObject.name);
        float restoration = DehydrationApi.CalculateThirstRestoration(__instance.gameObject);
        if (restoration <= 0f) return;

        __instance.OnConsumed += () =>
        {
            var holder = __instance.holderCharacter;
            var refs = holder != null ? holder.refs : null;
            var aff = refs != null ? refs.afflictions : null;

            if (aff != null)
            {
                aff.SubtractStatus(ThirstAffliction.DehydrationType, restoration);
            }
        };
    }
}

