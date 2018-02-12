using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PBCommon;
using PBBattle;
using PBLogin;
public class UdpPB {
	//返回给游戏的delegate
	public delegate void DelegateReceiveMessage<T>(T message);

	public DelegateReceiveMessage<UdpBattleStart> mes_battle_start{ get; set;}
	public DelegateReceiveMessage<UdpDownFrameOperations> mes_frame_operation{ get; set;}
	public DelegateReceiveMessage<UdpDownDeltaFrames> mes_delta_frame_data{ get; set;}
	public DelegateReceiveMessage<UdpDownGameOver> mes_down_game_over{ get; set;}

	private static UdpPB singleInstance;
	private MarsUdp _upd;
	private int mesNum = 0;
	public static UdpPB Instance()
	{
		// 如果类的实例不存在则创建，否则直接返回
		if (singleInstance == null)
		{
			singleInstance = new UdpPB();
		}
		return singleInstance;
	}

	private UdpPB()
	{

	}

	public void Destory(){
		singleInstance = null;

		if (_upd != null) {
			_upd.EndClientUdp ();
			_upd = null;
		}

		mes_frame_operation = null;
		mes_delta_frame_data = null;
		mes_down_game_over = null;
	}


	public void StartClientUdp(){
		mesNum = 0;
		_upd = new MarsUdp ();
		_upd.StartClientUdp (NetGlobal.Instance().serverIP);

		_upd.delegate_analyze_message = AnalyzeMessage;
	}
		
		
	public void SendBattleReady(int _uid, int _battleID){
		UdpBattleReady _ready = new UdpBattleReady ();
		_ready.uid = _uid;
		_ready.battleID = _battleID;
		_upd.SendMessage (CSData.GetSendMessage<UdpBattleReady>(_ready,CSID.UDP_BATTLE_READY));
	}


	public void SendOperation(){
		mesNum++;
	
		UdpUpPlayerOperations _up = new UdpUpPlayerOperations ();
		_up.mesID = mesNum;

		_up.operation = BattleData.Instance.selfOperation;

//		_up.operation = new PlayerOperation ();
//		_up.operation.battleID = BattleData.Instance.selfOperation.battleID;
//		_up.operation.move = BattleData.Instance.selfOperation.move;
//
//		if (BattleData.Instance.selfOperation.rightOperation != RightOpType.noop) {
//			_up.operation.operationID = BattleData.Instance.selfOperation.operationID;
//			_up.operation.rightOperation = BattleData.Instance.selfOperation.rightOperation;
//			_up.operation.operationValue1 = BattleData.Instance.selfOperation.operationValue1;
//			_up.operation.operationValue2 = BattleData.Instance.selfOperation.operationValue2;
//		}
			
		_upd.SendMessage (CSData.GetSendMessage<UdpUpPlayerOperations>(_up,CSID.UDP_UP_PLAYER_OPERATIONS));
	}


	public PlayerOperation ClonePlayerOperation(){
		PlayerOperation _operation = new PlayerOperation ();
		_operation.battleID = BattleData.Instance.selfOperation.battleID;
		_operation.move = BattleData.Instance.selfOperation.move;
		_operation.operationID = BattleData.Instance.selfOperation.operationID;
		_operation.rightOperation = BattleData.Instance.selfOperation.rightOperation;
		_operation.operationValue1 = BattleData.Instance.selfOperation.operationValue1;
		_operation.operationValue2 = BattleData.Instance.selfOperation.operationValue2;
		return _operation;
	}

	public void SendDeltaFrames(int _battleID,List<int> _frames){
		UdpUpDeltaFrames _framespb = new UdpUpDeltaFrames ();
	
		_framespb.battleID = _battleID;
		_framespb.frames.AddRange(_frames);

		_upd.SendMessage (CSData.GetSendMessage<UdpUpDeltaFrames>(_framespb,CSID.UDP_UP_DELTA_FRAMES));
	}


	public void SendGameOver(int _battleID){
		UdpUpGameOver _gameOver = new UdpUpGameOver ();
		_gameOver.battleID = _battleID;
		_upd.SendMessage (CSData.GetSendMessage<UdpUpGameOver>(_gameOver,CSID.UDP_UP_GAME_OVER));
	}


	public void AnalyzeMessage(SCID messageId,byte[] bodyData){
		switch (messageId) {
		case SCID.UDP_BATTLE_START:
			{
				UdpBattleStart pb_ReceiveMes = CSData.DeserializeData<UdpBattleStart>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_battle_start(pb_ReceiveMes);
				});
			}
			break;
		case SCID.UDP_DOWN_FRAME_OPERATIONS:
			{
				UdpDownFrameOperations pb_ReceiveMes = CSData.DeserializeData<UdpDownFrameOperations>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_frame_operation(pb_ReceiveMes);
				});
			}
			break;
		case SCID.UDP_DOWN_DELTA_FRAMES:
			{
				UdpDownDeltaFrames pb_ReceiveMes = CSData.DeserializeData<UdpDownDeltaFrames>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_delta_frame_data(pb_ReceiveMes);
				});
			}
			break;
		case SCID.UDP_DOWN_GAME_OVER:
			{
				UdpDownGameOver pb_ReceiveMes = CSData.DeserializeData<UdpDownGameOver>(bodyData);
				NetGlobal.Instance ().AddAction (()=>{
					mes_down_game_over(pb_ReceiveMes);
				});
			}
			break;
		default:
			break;
		}
	}
}
