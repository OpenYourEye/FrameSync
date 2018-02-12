using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBase : MonoBehaviour {

	public bool isBroken;

	private TextMesh bloodText;
	private int blood;//障碍血量
	[HideInInspector]
	public ShapeBase objShape;
	// Use this for initialization
	void Start () {
		
	}

	public void InitData(int _id,GameVector2 _logicPos,int _blood){
		isBroken = false;

		objShape = GetComponent<ShapeBase> ();
		objShape.InitSelf (ObjectType.obstacle,_id);
		objShape.SetPosition (_logicPos);
		transform.position = objShape.GetPositionVec3 ();

		blood = _blood;

		bloodText = transform.Find ("BloodNum").GetComponent<TextMesh> ();
		bloodText.text = blood.ToString ();
	}

	public bool BeAttackBroken(int _atk){
		blood--;
		if (blood <= 0) {
			isBroken = true;
			return true;
		} else {
			bloodText.text = blood.ToString ();
			return false;
		}
	}

	public void DestorySelf(){
		Destroy (gameObject);
	}
}
