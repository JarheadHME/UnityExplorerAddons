using System;
using HarmonyLib;
using UnityExplorer.CSConsole;

namespace UnityExplorerAddons.Patches
{
    [HarmonyPatch]
    internal class AddToCSConsolePatch
    {
        [HarmonyPatch(typeof(ConsoleController), nameof(ConsoleController.ResetConsole), new Type[] {typeof(bool) })]
        [HarmonyPostfix]
        public static void ChangeConsoleBaseClass()
        {
            var eval = ConsoleController.Evaluator;
            AccessTools.PropertySetter(typeof(ScriptEvaluator), "InteractiveBaseClass").Invoke(eval, [typeof(CSConsoleExtension)]);
        }
    }
}
