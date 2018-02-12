using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PBCommon;
using PBMatch;
public class MainCon : MonoBehaviour {

	public GameObject waitMatchObj;

	void Start(){
		waitMatchObj.SetActive (false);

		TcpPB.Instance ().mes_request_match_result = Message_Reauest_Match_Result;
		TcpPB.Instance ().mes_cancel_match_result = Message_Cancel_Match_Result;
		TcpPB.Instance ().mes_enter_battle = Message_Enter_Battle;
	}

	public void OnClickMatch(){
		//开始匹配
//		SceneManager.LoadScene (GameConfig.battleScene);
		TcpRequestMatch _mes = new TcpRequestMatch();
		_mes.uid = NetGlobal.Instance().userUid;
		_mes.roleID = 1;
		MarsTcp.Instance.SendMessage(CSData.GetSendMessage<TcpRequestMatch>(_mes,CSID.TCP_REQUEST_MATCH));
	}

	public void OnCliclCancelMatch(){
		//取消匹配
		TcpCancelMatch _mes = new TcpCancelMatch();
		_mes.uid = NetGlobal.Instance().userUid;
		MarsTcp.Instance.SendMessage(CSData.GetSendMessage<TcpCancelMatch>(_mes,CSID.TCP_CANCEL_MATCH));
	}

	void Message_Reauest_Match_Result(TcpResponseRequestMatch _result){
		waitMatchObj.SetActive (true);
	}

	void Message_Cancel_Match_Result(TcpResponseCancelMatch _result){
		waitMatchObj.SetActive (false);
	}

	void Message_Enter_Battle(PBBattle.TcpEnterBattle _mes){

		BattleData.Instance.UpdateBattleInfo(_mes.randSeed,_mes.battleUserInfo);

		ClearSceneData.LoadScene (GameConfig.battleScene);
	}


	void OnDestroy(){
		TcpPB.Instance ().mes_request_match_result = null;
		TcpPB.Instance ().mes_cancel_match_result = null;
		TcpPB.Instance ().mes_enter_battle = null;
	}
}
