/****************************************************
	文件：MapMgr.cs
	作者：Plane
	邮箱: 1785275942@qq.com
	日期：2019/03/15 9:10   	
	功能：地图管理器
*****************************************************/

using UnityEngine;

public class MapMgr : MonoBehaviour {
    private int waveIndex = 1;//默认生成第一波怪物
    private BattleMgr battleMgr;
    public TriggerData[] triggerArr;

    public void Init(BattleMgr battle) {
        battleMgr = battle;

        //实例化第一批怪物
        battleMgr.LoadMonsterByWaveID(waveIndex);

        PECommon.Log("Init MapMgr Done.");
    }

    public void TriggerMonsterBorn(TriggerData trigger, int waveIndex) {
        if (battleMgr != null) {
            BoxCollider co = trigger.gameObject.GetComponent<BoxCollider>();
            co.isTrigger = false;

            battleMgr.LoadMonsterByWaveID(waveIndex);
            battleMgr.ActiveCurrentBatchMonsters();
            battleMgr.triggerCheck = true;
        }
    }

    public bool SetNextTriggerOn() {
        waveIndex += 1;
        for (int i = 0; i < triggerArr.Length; i++) {
            if (triggerArr[i].triggerWave == waveIndex) {
                BoxCollider co = triggerArr[i].GetComponent<BoxCollider>();
                co.isTrigger = true;
                return true;
            }
        }

        return false;
    }

}
