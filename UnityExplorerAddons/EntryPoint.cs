using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;
using Il2CppInterop.Runtime.Injection;
using System.Linq;
using UnityEngine;
using UnityExplorerAddons.Modules;

namespace UnityExplorerAddons
{
    [BepInPlugin("JarheadHME.UnityExplorerAddons", "UnityExplorerAddons", VersionInfo.Version)]
    [BepInDependency("com.sinai.unityexplorer", BepInDependency.DependencyFlags.HardDependency)]
    internal class EntryPoint : BasePlugin
    {
        private Harmony _Harmony = null;

        public override void Load()
        {
            _Harmony = new Harmony($"{VersionInfo.RootNamespace}.Harmony");
            _Harmony.PatchAll();
            Logger.Info($"Plugin has loaded with {_Harmony.GetPatchedMethods().Count()} patches!");

            ClassInjector.RegisterTypeInIl2Cpp<ValueWatcher>();
            GameObject Watcher = new("ValueWatcher");
            GameObject.DontDestroyOnLoad( Watcher );
            Watcher.hideFlags = HideFlags.HideAndDontSave;
            Watcher.AddComponent<ValueWatcher>();
        }

        public override bool Unload()
        {
            _Harmony.UnpatchSelf();
            return base.Unload();
        }
    }
}
