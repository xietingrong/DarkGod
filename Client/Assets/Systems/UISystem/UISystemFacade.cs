using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace UISystem {
    /// <summary>
    /// 弹窗管理
    /// </summary>
    public class UISystemFacade : Singleton<UISystemFacade>,IWindowManager
    {


        /// <summary>等着要打开的界面</summary>
        private List<int> WaitForOpenList = new List<int>();

        #region WindowManager
        WindowManager windowManager = new WindowManager();
        public void Init()
        {
            windowManager.Init();
        }

        public GameObject OpenWindow(int windowType)
        {
            return windowManager.OpenWindow(windowType);
        }

        public WindowBase GetWindowByTypeId(int windowTypeId, int windowId = 0)
        {
            return windowManager.GetWindowByTypeId(windowTypeId,windowId);
        }

        public void CloseOpenWindowById(int windowId)
        {
            windowManager.CloseOpenWindowById(windowId);
        }
        public void CloseOpenWindowByTypeId(int windowTypeId)
        {
            windowManager.CloseOpenWindowByType(windowTypeId);
        }
        #endregion

    }
}