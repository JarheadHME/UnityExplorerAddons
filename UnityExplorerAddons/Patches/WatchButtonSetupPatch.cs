using HarmonyLib;
using UnityEngine;
using UnityExplorer;
using UnityExplorer.CacheObject;
using UnityExplorer.CacheObject.Views;
using UnityExplorerAddons.Modules;
using UniverseLib.UI;

namespace UnityExplorerAddons.Patches
{
    [HarmonyPatch]
    internal class WatchButtonSetupPatch
    {
        [HarmonyPatch(typeof(CacheObjectCell), nameof(CacheObjectCell.CreateContent))]
        [HarmonyPostfix]
        public static void AddWatchButton(CacheObjectCell __instance)
        {

            var copy = __instance.CopyButton;

            var parent = copy.Transform.parent.gameObject;
            UIFactory.SetLayoutElement(parent, minWidth: 30);

            var watchButton = UIFactory.CreateButton(copy.Transform.parent.gameObject, "WatchButton", "Watch", new Color(0.13f, 0.13f, 0.13f, 1f));
            UIFactory.SetLayoutElement(watchButton.Component.gameObject, minHeight: 25, minWidth: 33, flexibleWidth: 0);
            watchButton.ButtonText.color = Color.cyan;
            watchButton.ButtonText.fontSize = 10;
            watchButton.OnClick += () =>
            {
                if (__instance?.Occupant is not CacheMember)
                {
                    ExplorerCore.LogWarning("Cell that wanted to be watched doesn't have a Member to watch???");
                    return;
                }

                object[] args = null;
                // allow watching methods, but save the parameters you had entered when hitting watch
                if (__instance.Occupant is CacheMethod)
                {
                    var method = __instance.Occupant as CacheMethod;
                    args = method.Evaluator?.TryParseArguments();

                    if (method.HasArguments && (args == null || args.Length == 0))
                    {
                        ExplorerCore.LogWarning($"Method {method.NameForFiltering} requires arguments, and none have been placed.");
                        return;
                    }
                }

                ValueWatcher.SimpleWatches.Add((__instance.Occupant as CacheMember, args));
            };

        }
    }
}
