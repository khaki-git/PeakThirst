using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;

namespace PeakThirst.Patches
{
    [HarmonyPatch]
    internal static class GetEatenHydrationPatch
    {
        private static MethodInfo Apply => AccessTools.Method(typeof(GetEatenHydrationPatch), nameof(ApplyHydration));

        private static IEnumerable<MethodBase> TargetMethods()
        {
            foreach (var t in AccessTools.AllTypes())
            {
                var m = AccessTools.DeclaredMethod(t, "GetEaten", new[] { typeof(Character) });
                if (m != null) yield return m;
            }
        }

        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            var codes = new CodeMatcher(instructions, il);

            bool InsertedAt(string methodName)
            {
                var before = codes.Pos;
                codes.MatchForward(false, new CodeMatch(i =>
                    (i.opcode == OpCodes.Call || i.opcode == OpCodes.Callvirt) &&
                    i.operand is MethodInfo mi && mi.Name == methodName));
                if (!codes.IsValid)
                {
                    codes.Start().Advance(before);
                    return false;
                }
                codes.Insert(
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, Apply)
                );
                return true;
            }

            if (!InsertedAt("ThrowAchievement") && !InsertedAt("AddStatus") && !InsertedAt("DieInstantly"))
            {
                codes.End().Insert(
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, Apply)
                );
            }

            return codes.InstructionEnumeration();
        }

        private static void ApplyHydration(Character eater)
        {
            if (eater == null || !eater.IsLocal) return;
            eater.refs.afflictions.SubtractStatus(ThirstAffliction.DehydrationType, 500f);
        }
    }
}
