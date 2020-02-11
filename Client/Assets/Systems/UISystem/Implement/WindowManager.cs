using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace UISystem
{
    public class WindowManager 
    {

        private CanvasRoot canvasRoot;

        /// <summary>每个窗口的唯一ID</summary>
        private int instIndex = 0;

        public Dictionary<int, WindowCfg> WindowDict = new Dictionary<int, WindowCfg>();

        /// <summary>打开的界面 第一个为类型，第二个为ID</summary>
        Dictionary<int, Dictionary<int, WindowBase>> OpenedWindowDict = new Dictionary<int, Dictionary<int, WindowBase>>();

        public void Init()
        {
            GameObject go = Resources.Load<GameObject>("UIRoot");
            if (go != null)
            {
                GameObject instance = MonoBehaviour.Instantiate(go);
                canvasRoot = instance.GetComponent<CanvasRoot>();
            }
            Dictionary<int, WindowCfg> info= CfgSvc.Instance.WindowCfgDic;
            foreach (var winDict in info)
            {
                WindowDict.Add(winDict.Key, winDict.Value);
            }
        }

        /// <summary>
        /// 通过ID获取窗体
        /// </summary>
        /// <param name="windowTypeId">类型ID</param>
        /// <param name="windowId">唯一窗体ID</param>
        /// <returns></returns>
        public WindowBase GetWindowByTypeId(int windowTypeId, int windowId = 0)
        {
            Dictionary<int, WindowBase> tempDict;
            if (OpenedWindowDict.TryGetValue(windowTypeId, out tempDict))
            {
                if (windowId != 0)
                {
                    if (tempDict.ContainsKey(windowId))
                    {
                        return tempDict[windowId];
                    }
                }
                else
                {
                    if (tempDict.Count > 0)
                    {
                        return tempDict.Values.GetEnumerator().Current;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="windowType"></param>
        /// <returns></returns>
        public GameObject OpenWindow(int windowType)
        {
            WindowCfg info = null;
            WindowDict.TryGetValue(windowType, out info);
            WindowBase win;
            Dictionary<int, WindowBase> tempDict;

            if (info != null)
            {
                if (info.IsSingleton)
                {
                    if (OpenedWindowDict.TryGetValue(windowType, out tempDict))
                    {
                        if (tempDict.Count > 0)
                        {
                            tempDict.First().Value.ShowWidget();
                  
                            return tempDict.First().Value.gameObject;
                        }
                        else
                        {
                            win = CreateWindowByLayer(info.Layer, windowType);
                            tempDict.Add(win.WindowsId, win);
                        }
                    }
                    else
                    {
                        win = CreateWindowByLayer(info.Layer, windowType);
                        OpenedWindowDict.Add(windowType, new Dictionary<int, WindowBase>() { { win.WindowsId, win } });
                    }
                }
                else
                {
                    win = CreateWindowByLayer(info.Layer, windowType);
                    if (OpenedWindowDict.TryGetValue(windowType, out tempDict))
                    {
                        tempDict.Add(win.WindowsId, win);
                    }
                    else
                    {
                        OpenedWindowDict.Add(windowType, new Dictionary<int, WindowBase>() { { win.WindowsId, win } });
                    }
                }
                return win.gameObject;
            }
            return null;
        }

        /// <summary>
        /// 创建界面
        /// </summary>
        /// <param name="windowType"></param>
        /// <param name="canvas"></param>
        /// <returns></returns>
        public WindowBase CreateWindow(int windowType, Canvas canvas)
        {
            WindowCfg info;
            WindowDict.TryGetValue(windowType, out info);
            if (info != null)
            {
                //ResourceSystem.ResourceSystemFacade.Inst.LoadResource<GameObject>(info.PrefabName);
                GameObject go = Resources.Load<GameObject>("Prefab/" + info.PrefabName);

                if (go != null)
                {
                    GameObject returnObj = CanvasRoot.Instantiate(go, canvas.transform);

                    WindowBase win = returnObj.GetComponent<WindowBase>();
                    win.CurWindowInfo = info;
                    win.WindowsId = instIndex++;
                    return win;
                }
                else
                {
                   // DebugUtils.LogError("The Target not found");
                }
            }
            return null;
        }

        private WindowBase CreateWindowByLayer(int layer, int windowType)
        {
            switch (layer)
            {
                case 0:
                    return CreateWindow(windowType, canvasRoot.TopWindowCanvas);
                case 1:
                    return CreateWindow(windowType, canvasRoot.FirstWindowCanvas);
                case 2:
                    return CreateWindow(windowType, canvasRoot.SecondWindowCanvas);
                default:
                    return CreateWindow(windowType, canvasRoot.SecondWindowCanvas);
            }
        }

        /// <summary>
        /// 通过界面ID关闭打开的界面
        /// </summary>
        /// <param name="windowId"></param>
        public void CloseOpenWindowById(int windowId)
        {
            foreach (var winDict in OpenedWindowDict)
            {
                foreach (var id in winDict.Value.Keys)
                {
                    if (id == windowId)
                    {
                        if (winDict.Value[id].CurWindowInfo.Resident)
                        {
                            winDict.Value[id].HideWidget();
                        }
                        else
                        {
                            winDict.Value[id].CloseWindow();
                            winDict.Value.Remove(id);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 通过界面类型关闭打开的界面
        /// </summary>
        /// <param name="windowType"></param>
        public void CloseOpenWindowByType(int windowType)
        {
            Dictionary<int, WindowBase> winDict;
            if (OpenedWindowDict.TryGetValue(windowType, out winDict))
            {
                foreach (var win in winDict)
                {
                    if (win.Value.CurWindowInfo.Resident)
                    {
                        win.Value.HideWidget();
                    }
                    else
                    {
                        win.Value.CloseWindow();
                  
                    }
                }
                winDict.Clear();
            }
        }
    }
}