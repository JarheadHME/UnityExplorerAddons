using HarmonyLib;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityExplorer.UI;
using UnityExplorerAddons.Modules;
using UniverseLib;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace UnityExplorerAddons.Patches
{
    [HarmonyPatch]
    internal class AddNavBarButtonPatch
    {

        [HarmonyTargetMethod]
        public static MethodBase GetTarget()
        {
            return AccessTools.Method(typeof(UIManager), "InitUI");
        }

        [HarmonyPostfix]
        public static void AddWatchButton()
        {
            WatchNavButton = UIFactory.CreateButton(UIManager.NavbarTabButtonHolder, "WatchButton", "Watch");
            GameObject navBtn = WatchNavButton.Component.gameObject;
            navBtn.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(navBtn, false, true, true, true, 0, 0, 0, 5, 5, TextAnchor.MiddleCenter);
            UIFactory.SetLayoutElement(navBtn, minWidth: 60);

            RuntimeHelper.SetColorBlock(WatchNavButton.Component, UniversalUI.DisabledButtonColor, UniversalUI.DisabledButtonColor * 1.2f);
            WatchNavButton.OnClick += ToggleWatchWindow;

            GameObject txtObj = navBtn.transform.Find("Text").gameObject;
            txtObj.AddComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

            // Resize other button(s) to prevent squishing

            UIFactory.SetLayoutElement(UIManager.GetPanel(UIManager.Panels.ConsoleLog).NavButton.Component.gameObject, minWidth: 40);
            UIFactory.SetLayoutElement(UIManager.GetPanel(UIManager.Panels.Options).NavButton.Component.gameObject, minWidth: 70);
            UIFactory.SetLayoutElement(UIManager.GetPanel(UIManager.Panels.HookManager).NavButton.Component.gameObject, minWidth: 60);
        }

        static ButtonRef WatchNavButton = null;
        public static void ToggleWatchWindow()
        {
            ValueWatcher.IsVisible ^= true; // toggle
            Color newColor = ValueWatcher.IsVisible ? UniversalUI.EnabledButtonColor : UniversalUI.DisabledButtonColor;
            RuntimeHelper.SetColorBlock(WatchNavButton.Component, newColor, newColor * 1.2f);
        }
    }
}
