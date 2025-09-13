using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;

namespace PeakThirst.Patches
{
    [HarmonyPatch(typeof(WaterfallPusher))]
    internal static class WaterfallPusher_OnTriggerEnter_Patch
    {
        [HarmonyPatch("OnTriggerEnter")]
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var playCall = AccessTools.Method(typeof(SFX_PlayOneShot), nameof(SFX_PlayOneShot.Play));
            var getChar = AccessTools.Method(typeof(Component), "GetComponentInParent", Type.EmptyTypes, new[] { typeof(Character) });
            var apply = AccessTools.Method(typeof(WaterfallPusher_OnTriggerEnter_Patch), nameof(ApplyHydration));

            var matcher = new CodeMatcher(instructions, il)
                .MatchForward(false, new CodeMatch(ci => ci.Calls(playCall)));

            if (!matcher.IsValid) return matcher.InstructionEnumeration();

            matcher.Advance(1).Insert(
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Callvirt, getChar),
                new CodeInstruction(OpCodes.Call, apply)
            );

            return matcher.InstructionEnumeration();
        }

        private static void ApplyHydration(Character c)
        {
            if (c == null) return;
            c.refs.afflictions.SubtractStatus(ThirstAffliction.DehydrationType, 500f);
        }
    }
}
