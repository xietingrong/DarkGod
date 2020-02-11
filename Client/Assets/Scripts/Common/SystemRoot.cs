/****************************************************
    文件：SystemRoot.cs
	作者：Plane
    邮箱: 1785275942@qq.com
    日期：2018/12/4 5:45:48
	功能：业务系统基类
*****************************************************/

using UnityEngine;

public class SystemRoot : MonoBehaviour {
    protected ResSvc resSvc;
    protected AudioSvc audioSvc;
    protected NetSvc netSvc;
    protected TimerSvc timerSvc;

    public virtual void InitSys() {
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance;
        netSvc = NetSvc.Instance;
        timerSvc = TimerSvc.Instance;
    }
}