/****************************************************
文件：CfgSvc.cs
作者：xietingrong
邮箱: 592183492@qq.com
日期：21:48:38 2020/02/09
功能：配置数据服务
****************************************************/
using UnityEngine;
using System.Collections.Generic;

public class CfgSvc{
	private static  CfgSvc instance = null;
	public static CfgSvc Instance {
		get {
			if (instance == null) {
				instance = new CfgSvc();
			}
			return instance;
		}
	}
	#region  WindowCfg配置
	private Dictionary<int, WindowCfg> _WindowCfg = null;

	public Dictionary<int, WindowCfg> WindowCfgDic{
		get{
			if (_WindowCfg == null){
				_WindowCfg = ReadTable.Read<Dictionary<int, WindowCfg> >("WindowCfg");
			}
			return _WindowCfg;
		}
	}
	public WindowCfg GetWindowCfgDic(int id){
		WindowCfg trc = null;
		if(WindowCfgDic.TryGetValue(id, out trc))  {
			return trc;
		}
		return null;
	}
	#endregion
	public void Init()
	{
		_WindowCfg = ReadTable.Read<Dictionary<int, WindowCfg> >("WindowCfg");
	}
}
#region  WindowCfg类
public class WindowCfg{
	/// <summary>
	///索引号
	/// </summary>
	public readonly int id;
	/// <summary>
	///名称
	/// </summary>
	public readonly string Name;
	/// <summary>
	///预制体名称
	/// </summary>
	public readonly string PrefabName;
	/// <summary>
	///层级
	/// </summary>
	public readonly int Layer;
	/// <summary>
	///是否常驻
	/// </summary>
	public readonly bool Resident;
	/// <summary>
	///否只有一个是
	/// </summary>
	public readonly bool IsSingleton;
}
#endregion
