namespace UnityExplorerAddons.Modules
{
    internal interface IExpressionHolder 
    {
        object GetObject();
        string Invoke();
    }
}
