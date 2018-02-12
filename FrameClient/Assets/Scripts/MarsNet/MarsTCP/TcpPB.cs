using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PBCommon;
using PBLogin;
using PBMatch;
using PBBattle;
public class TcpPB {

	//返回给游戏的delegate
	public delegate void DelegateReceiveMessage<T>(T message);

	public DelegateReceiveMessage<TcpResponseLogin> mes_login_result{ get; set;}
	public DelegateReceiveMessage<TcpResponseRequestMatch> mes_request_match_result{ get; set;}
	public DelegateReceiveMessage<TcpResponseCancelMatch> mes_cancel_match_result{ get; set;}
	public DelegateReceiveMessage<TcpEnterBattle> mes_enter_battle{ get; set;}
	private static TcpPB instance;
	public static TcpPB Instance()
	{
		// 如果类的实例不存在则创建，否则直接返回
		if (instance == null)
		{
			instance = new TcpPB();
		}
		return instance;
	}

	private TcpPB()
	{

	}

	public void Destory(){
		instance = null;
	}

	public void AnalyzeMessage(SCID messageId,byte[] bodyData){
		switch (messageId) {
		case SCID.TCP_RESPONSE_LOGIN:
			{
				TcpResponseLogin pb_ReceiveMes = CSData.DeserializeData<TcpResponseLogin>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_login_result(pb_ReceiveMes);
				});
			}
			break;
		case SCID.TCP_RESPONSE_REQUEST_MATCH:
			{
				TcpResponseRequestMatch pb_ReceiveMes = CSData.DeserializeData<TcpResponseRequestMatch>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_request_match_result(pb_ReceiveMes);
				});
			}
			break;
		case SCID.TCP_RESPONSE_CANCEL_MATCH:
			{
				TcpResponseCancelMatch pb_ReceiveMes = CSData.DeserializeData<TcpResponseCancelMatch>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_cancel_match_result(pb_ReceiveMes);
				});
			}
			break;
		case SCID.TCP_ENTER_BATTLE:
			{
				TcpEnterBattle pb_ReceiveMes = CSData.DeserializeData<TcpEnterBattle>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_enter_battle(pb_ReceiveMes);
				});
			}
			break;
		default:
			break;
		}
	}


}


