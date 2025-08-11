using UnityExplorerAddons.API;

namespace UnityExplorerAddons.Modules
{
    internal class ExpressionHolder<T> : IExpressionHolder 
    {
        internal readonly T obj;
        internal readonly ExpressionWatcherDelegate<T> func;

        internal ExpressionHolder(ExpressionWatcherDelegate<T> func, T obj = default(T))
        {
            this.obj = obj;
            this.func = func;
        }

        public object GetObject() => obj;
        public string Invoke() => func(obj);
    }
}
