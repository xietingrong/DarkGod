using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace UISystem
{
    /// <summary>
    /// 弹窗接口
    /// </summary>
    public interface IWindow
    {
        /// <summary>
        /// 生成界面，初始化界面相关的控件
        /// </summary>
        void InitWidget();

        /// <summary>
        /// 注册监听
        /// </summary>
        void RegisterListener();

        /// <summary>
        /// 移除监听
        /// </summary>
        void Removelistener();

        /// <summary>
        /// 刷新界面
        /// </summary>
        void RefreshWindow();

        /// <summary>
        /// 关闭窗口，同时通知弹窗管理 窗口已关闭
        /// </summary>
        void CloseWindow();

        /// <summary>
        /// 清除界面
        /// </summary>
        void ReleaseWidget();

        /// <summary>
        /// 隐藏界面
        /// </summary>
        void HideWidget();

    }
}