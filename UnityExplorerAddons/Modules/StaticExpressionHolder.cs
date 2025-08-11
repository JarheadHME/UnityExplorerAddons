using UnityExplorerAddons.API;

namespace UnityExplorerAddons.Modules
{
    internal class StaticExpressionHolder : IExpressionHolder
    {
        internal StaticExpressionDelegate func;

        public StaticExpressionHolder(StaticExpressionDelegate func)
        {
            this.func = func;
        }

        public object GetObject() => null;

        public string Invoke()
        {
            return func();
        }
    }
}
