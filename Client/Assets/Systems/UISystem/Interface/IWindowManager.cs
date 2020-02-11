using UnityEngine;
using System.Collections;

namespace UISystem
{
    /// <summary>
    /// 窗体管理器接口
    /// </summary>
    public interface IWindowManager:IInitable
    {

        /// <summary>
        /// 打开界面
        /// </summary>
        /// <param name="windowType"></param>
        /// <returns></returns>
         GameObject OpenWindow(int windowType);

        /// <summary>
        /// 通过ID获取窗体
        /// </summary>
        /// <param name="windowTypeId">类型ID</param>
        /// <param name="windowId">唯一窗体ID</param>
        /// <returns></returns>
        WindowBase GetWindowByTypeId(int windowTypeId, int windowId = 0);

        /// <summary>
        /// 通过界面ID关闭打开的界面
        /// </summary>
        /// <param name="windowId"></param>
        void CloseOpenWindowById(int windowId);
    }
}