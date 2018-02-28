using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MarsClickBtn : MonoBehaviour {
	private EventTrigger _EventTri;
	private Image btnImage;
	void Start () {
		_EventTri = GetComponent<EventTrigger> ();
		btnImage = GetComponent<Image> ();
	}
	
	public void EnableButton(){
		_EventTri.enabled = true;
		btnImage.raycastTarget = true;
	}

	public void DisableButton(){
		_EventTri.enabled = false;
		btnImage.raycastTarget = true;
	}

	public void OnClickDown(){
		btnImage.color = Color.gray;

		if (gameObject.tag.Equals("NormalAttackButton")) {
			//普通攻击
			RoleBase _role = BattleCon.Instance.roleManage.GetRoleFromBattleID (BattleData.Instance.battleID);
			if (_role.IsCloudAttack()) {
				BattleData.Instance.UpdateRightOperation (PBBattle.RightOpType.rop1,0,0);	
			}
		}
		else if (gameObject.tag.Equals("BtnGameOver")) {
			BattleCon.Instance.OnClickGameOver ();
		}

	}

	public void OnClickUp(){
		btnImage.color = Color.white;
	}
}
