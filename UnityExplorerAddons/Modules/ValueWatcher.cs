using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityExplorer.CacheObject;
using UnityExplorer;
using UniverseLib;

namespace UnityExplorerAddons.Modules
{
    internal class ValueWatcher : MonoBehaviour
    {
        public static bool IsUniExpUIActive => UnityExplorer.UI.UIManager.ShowMenu;

        public static readonly List<(CacheMember, object[])> SimpleWatches = new();
        public static readonly List<IExpressionHolder> ExpressionWatches = new();
        private static readonly List<int> m_simpleToRemove = new List<int>();
        private static readonly List<int> m_expToRemove = new List<int>();

        public int ScreenWidth = 1920;
        public int ScreenHeight = 1080;
        public int Width = 400;
        public int Height = 720;
        public static int LineDifference = 22;

        public static bool IsVisible = false;

        public void Awake()
        {
            ScreenWidth = Screen.width;
            ScreenHeight = Screen.height;
            Width = ScreenWidth / 4;
            Height = (int)(ScreenHeight / 2.5);
        }

        public void OnGUI()
        {
            if (!IsVisible) return;

            var xpos = ScreenWidth - Width;
            var ypos = ScreenHeight / 3;
            GUI.Box(new Rect(xpos, ypos, Width, Height), "Watch List");

            RemoveQueuedRemovals();

            // Generate WatchList output

            int index = 0;
            int currYpos = ypos + LineDifference + 10; // extra spacing from the pane label
            if (SimpleWatches.Count > 0 )
            {
                GUI.Label(new Rect(xpos + 5, currYpos, Width - 10, LineDifference), "Simple Watches");
                currYpos = currYpos + LineDifference;

                for (int i = 0; i < SimpleWatches.Count; i++)
                {
                    var pair = SimpleWatches[i];
                    CacheMember watch = pair.Item1;
                    UnityEngine.Object obj = null;

                    if (!watch.IsStatic && watch.DeclaringInstance.GetType().IsSubclassOf(typeof(UnityEngine.Object)))
                    {
                        // make sure the object instance still exists
                        obj = watch.DeclaringInstance.TryCast<UnityEngine.Object>();
                        if (obj == null)
                        {
                            ExplorerCore.LogWarning($"Object for Watch index {i} ({watch.NameForFiltering}) destroyed");
                            m_simpleToRemove.Add(i);
                            continue;
                        }
                    }

                    string value;
                    if (watch is CacheMethod)
                    {
                        var method = watch as CacheMethod;
                        try
                        {                                                           // vvvvvvvvvv is cached parameters from when watch was pressed
                            value = method.MethodInfo.Invoke(method.DeclaringInstance, pair.Item2)?.ToString();
                        }
                        catch (Exception e)
                        {
                            ExplorerCore.LogWarning($"Simple watch (method) index {i} ({watch.NameForFiltering}) errored, removing.\n{e}");
                            m_simpleToRemove.Add(i);
                            continue;
                        }
                        
                    }
                    else
                    {
                        watch.Evaluate();

                        // attach object name if it's a unity object
                        var valobj = watch.Value?.TryCast<UnityEngine.Object>();
                        if (valobj != null)
                        {
                            value = $"{valobj.name} ({watch.Value.ToString()})";
                        }
                        else
                        {
                            value = watch.Value?.ToString();
                        }
                    }

                    StringBuilder text = new StringBuilder();

                    text.Append($"{watch.NameForFiltering}: {value}");

                    if (obj != null)
                        text.Append($" - {obj.name}");

                    CreateWatchEntry(currYpos, LineDifference, text.ToString(), index++, false);
                    currYpos += LineDifference;
                }

                // add another spacer line
                currYpos += LineDifference;
            }

            index = 0;

            if (ExpressionWatches.Count > 0 )
            {
                GUI.Label(new Rect(xpos + 5, currYpos, Width - 10, LineDifference), "Expression Watches");
                currYpos = currYpos + LineDifference;

                for (int i = 0; i < ExpressionWatches.Count; i++)
                {
                    var expPair = ExpressionWatches[i];
                    var instance = expPair.GetObject();

                    if (instance != null && instance.GetType().IsSubclassOf(typeof(UnityEngine.Object)) && instance.TryCast<UnityEngine.Object>() == null)
                    {
                        ExplorerCore.LogWarning($"Expression Watch index {i} instance is null");
                        m_simpleToRemove.Add(i);
                        continue;
                    }

                    try
                    {
                        string result = expPair.Invoke();
                        int height = LineDifference * result.Split('\n').Length;
                        CreateWatchEntry(currYpos, height, result, index++, true);
                        currYpos += height;
                        //output.AppendLine($"[{index++}] {result}");
                    }
                    catch (Exception ex)
                    {
                        ExplorerCore.LogWarning($"Exception occurred while evaluating Expression Watch index {i}, removing.\n{ex}");
                        m_simpleToRemove.Add(i);
                        continue;
                    }

                }
            }
        }

        private void RemoveQueuedRemovals()
        {
            for (int i = m_simpleToRemove.Count - 1; i >= 0; i--)
                SimpleWatches.RemoveAt(m_simpleToRemove[i]);
            m_simpleToRemove.Clear();

            for (int i = m_expToRemove.Count - 1; i >= 0; i--)
                ExpressionWatches.RemoveAt(m_expToRemove[i]);
            m_expToRemove.Clear();
        }

        internal void CreateWatchEntry(int ypos, int height, string info, int index, bool exp)
        {
            int baseXpos = ScreenWidth - Width;
            GUI.Label(new Rect(baseXpos + 5, ypos, int.MaxValue, height), $"[{index}] {info}");

            if (IsUniExpUIActive)
            {
                if (GUI.Button(new Rect(baseXpos - 25, ypos-2, 22, 22), " X"))
                {
                    if (exp)
                        m_expToRemove.Add(index);
                    else
                        m_simpleToRemove.Add(index);
                }
            }

        }
    }
}
