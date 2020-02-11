using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public enum MaskType
    {
        //啥都没有
        NONE = 0,
        //背景
        BACKGROUND = 1,
        //蒙版
        MASK = 2,
    }
    /// <summary>
    /// 弹窗基类
    /// </summary>
    public abstract class WindowBase : MonoBehaviour, IWindow
    {
        public WindowCfg CurWindowInfo;

        /// <summary>
        /// 弹框类型
        /// </summary>
        protected int WindowsType
        {
            get
            {
                return CurWindowInfo.id;
            }
        }

        public int WindowsId { get; set; }

        protected void Awake()
        {
            InitWidget();
        }

        /// <summary>
        /// 生成界面，初始化界面相关的控件
        /// </summary>
        public abstract void InitWidget();

        /// <summary>
        /// 注册监听
        /// </summary>
        public abstract void RegisterListener();

        /// <summary>
        /// 刷新界面
        /// </summary>
        public abstract void RefreshWindow();

        void OnEnable()
        {
            RegisterListener();
            OnShow();
            RefreshWindow();
        }

        /// <summary>
        /// 关闭窗口，同时通知弹窗管理 窗口已关闭
        /// </summary>
        public virtual void CloseWindow()
        {
            ReleaseWidget();
            Removelistener();
            StopAllCoroutines();
            Destroy(this.gameObject);
        }

        /// <summary>
        /// 清除界面
        /// </summary>
        public abstract void ReleaseWidget();

        void OnDisable()
        {
            Removelistener();
            OnHide();
        }

        /// <summary>
        /// 显示界面
        /// </summary>
        public virtual void ShowWidget()
        {
            this.gameObject.SetActive(true);
        }

        /// <summary>
        /// 隐藏界面
        /// </summary>
        public virtual void HideWidget()
        {
            this.gameObject.SetActive(false);
        }

        /// <summary>
        /// 隐藏界面时调用
        /// </summary>
        public abstract void OnHide();

        /// <summary>
        /// 显示界面时调用
        /// </summary>
        public abstract void OnShow();

        /// <summary>
        /// 移除监听
        /// </summary>
        public abstract void Removelistener();

        void OnDestroy()
        {
            ReleaseWidget();
            Removelistener();
        }

        public abstract void OnEnter();
        public abstract void OnPause();
        public abstract void OnResume();
        public abstract void OnExit();
    }
}