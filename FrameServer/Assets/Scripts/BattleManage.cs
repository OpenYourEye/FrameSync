using System.Collections;
using System.Collections.Generic;

public class BattleManage {

	private int battleID;
	private Dictionary<int,BattleCon> dic_battles;

	private static BattleManage instance = null;
	public static BattleManage Instance
	{
		get{ 
			if (instance == null) {
				instance = new BattleManage ();
			}
			return instance;
		}
	}

	private BattleManage(){
		battleID = 0;
		dic_battles = new Dictionary<int, BattleCon> ();
	}

	public void Creat(){

	}


	public void Destroy(){

		foreach (var item in dic_battles) {
			item.Value.DestroyBattle ();
		}

		dic_battles.Clear ();
		instance = null;
	}

	public void BeginBattle(List<MatchUserInfo> _battleUser){
		
		battleID++;
		BattleCon _battle = new BattleCon ();
		_battle.CreatBattle (battleID,_battleUser);

		dic_battles [battleID] = _battle;

		UnityEngine.Debug.Log ("开始战斗。。。。。" + battleID);	
	}


	public void FinishBattle(int _battleID){
		dic_battles.Remove (_battleID);
		UnityEngine.Debug.Log ("战斗结束。。。。。" + _battleID);
	}


}
