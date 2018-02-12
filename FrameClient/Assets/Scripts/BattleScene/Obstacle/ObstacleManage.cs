using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManage : MonoBehaviour {

	public bool initFinish;

	private int obstacleID;
	private Transform obstacleParent;
	private Dictionary<string,GameObject> dic_preObstacles;
	private Dictionary<int,ObstacleBase> dic_obstacles;

	private List<ObstacleBase> list_brokenObs;
	public void InitData(Transform _obstParent,GameVector2[] _roleGrid){
		initFinish = false;
		obstacleParent = _obstParent;
		obstacleID = 0;
		dic_obstacles = new Dictionary<int, ObstacleBase> ();
		list_brokenObs = new List<ObstacleBase> ();
		StartCoroutine (CreatObstacle(_roleGrid));
	}

	IEnumerator CreatObstacle(GameVector2[] _roleGrid){
		dic_preObstacles = new Dictionary<string, GameObject> ();
		GameObject[] preObstacle = Resources.LoadAll<GameObject> ("BattleScene/Obstacle");
		foreach (var item in preObstacle) {
			dic_preObstacles [item.name] = item;
		}
		yield return new WaitForEndOfFrame ();

		bool[,] mapGrid = new bool[BattleData.mapRow,BattleData.mapColumn];

		foreach (var item in _roleGrid) {
			mapGrid [item.x,item.y] = true;
		}

		int _creatNum = 0;
		for (int i = 0; i < BattleData.mapRow; i++) {
			for (int j = 0; j < BattleData.mapColumn; j++) {
				if (mapGrid [i,j]) {
					continue;
				} else {
					obstacleID++;
					GameObject _base = Instantiate (dic_preObstacles["ObstacleBase"],obstacleParent);

					string _modleStr = "ObsModel";
					Instantiate (dic_preObstacles[_modleStr],_base.transform);

					GameVector2 _pos = BattleData.Instance.GetMapGridCenterPosition(i,j);
					ObstacleBase _obstacle = _base.GetComponent<ObstacleBase> ();
					_obstacle.InitData (obstacleID,_pos,1);
					dic_obstacles [obstacleID] = _obstacle;
				}

				_creatNum++;
				if (_creatNum % 3 == 0) {
					yield return new WaitForEndOfFrame ();
				}
			}
		}
			
		initFinish = true;
	}

	public bool CollisionCorrection(GameVector2 _tVec,int _tRadius,out GameVector2 _tVecCC){
		bool _result = false;

		_tVecCC = _tVec;
		foreach (var item in dic_obstacles.Values) {
			if (!item.objShape.IsInBaseCircleDistance (_tVecCC,_tRadius)) {
				continue;
			}

			GameVector2 _amend;
			if (item.objShape.IsCollisionCircleCorrection (_tVecCC, _tRadius, out _amend)) {
				_tVecCC += _amend;
				_result = true;
			}
		}
		return _result;
	}



	//点攻击
	public bool AttackObstacle(ObjectUid _attacker,GameVector2 _attackPoint,int _attackRadius,int _attackNum){

		bool isAttack = false;
		foreach (var item in dic_obstacles.Values) {
			if (item.isBroken || !item.objShape.IsInBaseCircleDistance(_attackPoint,_attackRadius)) {
				continue;
			}

			if (item.objShape.IsCollisionCircle(_attackPoint,_attackRadius)) {
				if (item.BeAttackBroken(_attackNum)) {
					//击碎
					list_brokenObs.Add(item);
					isAttack = true;
				}
			}
		}

		return isAttack;
	}

	public void Logic_Destory(){
		if (list_brokenObs.Count > 0) {
			for (int i = 0; i < list_brokenObs.Count; i++) {
				ObstacleBase _obs = list_brokenObs [0];
				dic_obstacles.Remove (_obs.objShape.ObjUid.objectID);
				_obs.DestorySelf ();
			}

			list_brokenObs.Clear ();
		}
	}
	void OnDestroy ()
	{

	}

}
