using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : MonoBehaviour {
	[HideInInspector]
	public ShapeBase objShape;

//	private int owerID;
	public int speed;
	public int life;
	private int curLife;

	private Vector3 renderPosition;
	private GameVector2 logicSpeed;
	public void InitData(int _owerID,int _id,GameVector2 _logicPos,int _moveDir){

//		owerID = _owerID;

		objShape = GetComponent<ShapeBase> ();
		objShape.InitSelf (ObjectType.bullet,_id);
		objShape.SetPosition (_logicPos);
		transform.position = objShape.GetPositionVec3 (0.5f);

		logicSpeed = speed * BattleData.Instance.GetSpeed (_moveDir);
		curLife = life;
	}

	public int GetBulletID(){
		return objShape.ObjUid.objectID;
	}
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position,renderPosition,0.4f);
	}

	public virtual void Logic_Move(){
		curLife--;
		if (curLife > 0) {
			GameVector2 _targetPos = objShape.GetPosition () + logicSpeed;
			objShape.SetPosition (_targetPos);
			renderPosition = objShape.GetPositionVec3 (0.5f);
		} else {

		}
	}

	public virtual void Logic_Collision(){
		if (BattleCon.Instance.obstacleManage.AttackObstacle(objShape.ObjUid,objShape.GetPosition(),objShape.GetRadius(),1)) {
			curLife = 0;
		}
	}

	public virtual bool Logic_Destory(){
		if (curLife <= 0) {
			Destroy (gameObject);
			return true;
		} else {
			return false;
		}
	}
}
