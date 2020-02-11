/****************************************************
	文件：IState.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2019/03/19 6:38   	
	功能：状态接口
*****************************************************/

public interface IState {
    void Enter(EntityBase entity, params object[] args);//可变参数

    void Process(EntityBase entity, params object[] args);

    void Exit(EntityBase entity, params object[] args);
}

public enum AniState {
    None,
    Born,
    Idle,
    Move,
    Attack,
    Hit,
    Die
}
