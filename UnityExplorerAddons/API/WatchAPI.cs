using UnityExplorerAddons.Modules;

namespace UnityExplorerAddons.API
{
    public delegate string StaticExpressionDelegate();
    public delegate string ExpressionWatcherDelegate<T>(T instance);
    public static class WatchAPI
    {
        public static void Watch(StaticExpressionDelegate del)
        {
            StaticExpressionHolder holder = new(del);
            ValueWatcher.ExpressionWatches.Add(holder);
        }
        public static void Watch<T>(T instance, ExpressionWatcherDelegate<T> del)
        {
            ExpressionHolder<T> holder = new(del, instance);
            ValueWatcher.ExpressionWatches.Add(holder);
        }

        public static void Watch<T>(object instance, ExpressionWatcherDelegate<T> del)
        {
            ExpressionHolder<T> holder = new(del, (T)instance);
            ValueWatcher.ExpressionWatches.Add(holder);
        }
    }
}
