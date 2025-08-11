using UnityExplorer.CSConsole;
using UnityExplorerAddons.API;

namespace UnityExplorerAddons
{
    public class CSConsoleExtension : ScriptInteraction
    {
        public static void Watch(StaticExpressionDelegate del) => WatchAPI.Watch(del);
        public static void Watch<T>(T instance, ExpressionWatcherDelegate<T> del) => WatchAPI.Watch<T>(instance, del);
        public static void Watch<T>(object instance, ExpressionWatcherDelegate<T> del) => WatchAPI.Watch<T>(instance, del);
    }
}
