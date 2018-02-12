using System.Collections;
using System.Collections.Generic;
using System.Threading;
using PBBattle;
using PBCommon;
public class BattleCon {
	private int battleID;
	private Dictionary<int,int> dic_battleUserUid;
	private Dictionary<int,ClientUdp> dic_udp;

	private Dictionary<int,bool> dic_battleReady;

	private bool _isRun = false;
	private bool isBeginBattle = false;
	private int frameNum;
	private int lastFrame;

	private PlayerOperation[] frameOperation;//记录当前帧的玩家操作
	private int[] playerMesNum;//记录玩家的包id
	private bool[] playerGameOver;//记录玩家游戏结束
	private bool oneGameOver;
	private bool allGameOver;

	private Dictionary<int,AllPlayerOperation> dic_gameOperation = new Dictionary<int, AllPlayerOperation>();


	private Timer waitBattleFinish;
	private float finishTime;//结束倒计时
	public void CreatBattle(int _battleID,List<MatchUserInfo> _battleUser){
		int randSeed = UnityEngine.Random.Range (0, 100);
		ThreadPool.QueueUserWorkItem((obj)=>{
			battleID = _battleID;
			dic_battleUserUid = new Dictionary<int, int> ();
			dic_udp = new Dictionary<int, ClientUdp> ();
			dic_battleReady = new Dictionary<int, bool>();

			int userBattleID = 0;

			TcpEnterBattle _mes = new TcpEnterBattle();
			_mes.randSeed = randSeed;
			for (int i = 0; i < _battleUser.Count; i++) {
				int _userUid = _battleUser [i].uid;
				userBattleID++;

				dic_battleUserUid [_userUid] = userBattleID;

				string _ip = UserManage.Instance.GetUserInfo (_userUid).socketIp;
				var _upd = new ClientUdp ();

				_upd.StartClientUdp (_ip,_userUid);	
				_upd.delegate_analyze_message = AnalyzeMessage;
				dic_udp [userBattleID] = _upd;
				dic_battleReady[userBattleID] = false;

				BattleUserInfo _bUser = new BattleUserInfo();
				_bUser.uid = _userUid;
				_bUser.battleID = userBattleID;
				_bUser.roleID = _battleUser [i].roleID;

				_mes.battleUserInfo.Add(_bUser);
			}

			for (int i = 0; i < _battleUser.Count; i++) {
				int _userUid = _battleUser [i].uid;
				string _ip = UserManage.Instance.GetUserInfo (_userUid).socketIp;
				ServerTcp.Instance.SendMessage(_ip,CSData.GetSendMessage<TcpEnterBattle>(_mes,SCID.TCP_ENTER_BATTLE));
			}
		},null);
	}

	public void DestroyBattle(){
		foreach (var item in dic_udp.Values) {
			item.EndClientUdp ();
		}

		_isRun = false;
	}

	private void FinishBattle(){
		foreach (var item in dic_udp.Values) {
			item.EndClientUdp ();
		}

		BattleManage.Instance.FinishBattle (battleID);
	}

	private void CheckBattleBegin(int _userBattleID){

		if (isBeginBattle) {
			return;
		}

		dic_battleReady[_userBattleID] = true;

		isBeginBattle = true;
		foreach (var item in dic_battleReady.Values) {
			isBeginBattle = isBeginBattle && item;
		}

		if (isBeginBattle) {
			//开始战斗
			BeginBattle();
		}
	}

	void BeginBattle(){
		frameNum = 0;
		lastFrame = 0;
		_isRun = true;
		oneGameOver = false;
		allGameOver = false;

		int playerNum = dic_battleUserUid.Keys.Count;

		frameOperation = new PlayerOperation[playerNum];
		playerMesNum = new int[playerNum];
		playerGameOver = new bool[playerNum];
		for (int i = 0; i < playerNum; i++) {
			frameOperation [i] = null;
			playerMesNum [i] = 0;
			playerGameOver [i] = false;
		}
			
		Thread _threadSenfd = new Thread(Thread_SendFrameData);  
		_threadSenfd.Start();
	}


	private void Thread_SendFrameData(){
		//向玩家发送战斗开始
		bool isFinishBS = false;
		while (!isFinishBS) {
			UdpBattleStart _btData = new UdpBattleStart ();
			byte[] _data = CSData.GetSendMessage<UdpBattleStart> (_btData, SCID.UDP_BATTLE_START);
			foreach (var item in dic_udp) {
				item.Value.SendMessage (_data);
			}

			bool _allData = true;
			for (int i = 0; i < frameOperation.Length; i++) {
				if (frameOperation[i] == null) {
					_allData = false;
					break;
				}
			}

			if (_allData) {
				UnityEngine.Debug.Log ("战斗服务器:收到全部玩家的第一次操作数据....");
				frameNum = 1;

				isFinishBS = true;
			}

			Thread.Sleep (500);
		}

		UnityEngine.Debug.Log ("开始发送帧数据～～～～");

		while (_isRun) {
			UdpDownFrameOperations _dataPb = new UdpDownFrameOperations ();
			if (oneGameOver) {
				_dataPb.frameID = lastFrame;
				_dataPb.operations = dic_gameOperation [lastFrame];
			} else {
				_dataPb.operations = new AllPlayerOperation ();
				_dataPb.operations.operations.AddRange (frameOperation);
				_dataPb.frameID = frameNum;
				dic_gameOperation [frameNum] = _dataPb.operations;
				lastFrame = frameNum;
				frameNum++;
			}

			byte[] _data = CSData.GetSendMessage<UdpDownFrameOperations> (_dataPb, SCID.UDP_DOWN_FRAME_OPERATIONS);
			foreach (var item in dic_udp) {
				int _index = item.Key - 1;
				if (!playerGameOver [_index]) {
					item.Value.SendMessage (_data);	
				} 
			}

			Thread.Sleep (ServerConfig.frameTime);
		}

		UnityEngine.Debug.Log ("帧数据发送线程结束.....................");
	}

	public void UpdatePlayerOperation(PlayerOperation _operation,int _mesNum){
		int _index = _operation.battleID - 1;
		//		Debug.Log ("收到玩家操作:" + _index + "," + _mesNum + "," + playerMesNum [_index]);
		if (_mesNum > playerMesNum [_index]) {
			frameOperation [_index] = _operation;
			playerMesNum [_index] = _mesNum;
		} else {
			//早期的包就不记录了
		}
	}

	public void UpdatePlayerGameOver(int _battleId){

		oneGameOver = true;

		int _index = _battleId - 1;
		playerGameOver [_index] = true;

		allGameOver = true;
		for (int i = 0; i < playerGameOver.Length; i++) {
			if (playerGameOver[i] == false) {
				allGameOver = false;
				break;
			}
		}

		if (allGameOver) {
//			UnityEngine.Debug.Log ("战斗即将结束咯......");
			_isRun = false;

			finishTime = 2000f;
			if (waitBattleFinish == null) {
				waitBattleFinish = new Timer (new TimerCallback(WaitClientFinish),null,1000,1000);	
			}
		}
	}

	void WaitClientFinish(object snder){
//		UnityEngine.Debug.Log ("等待客户端结束～");
		finishTime -= 1000f;
		if (finishTime <= 0) {
			waitBattleFinish.Dispose ();
			FinishBattle ();
//			UnityEngine.Debug.Log ("战斗结束咯......");
		}
	}

	public void AnalyzeMessage(CSID messageId,byte[] bodyData){
		switch (messageId) {
		case CSID.UDP_BATTLE_READY:
			{
				//接收战斗准备
				UdpBattleReady _mes = CSData.DeserializeData<UdpBattleReady> (bodyData);
				CheckBattleBegin (_mes.battleID);
				dic_udp [_mes.battleID].RecvClientReady (_mes.uid);
			}
			break;
		case CSID.UDP_UP_PLAYER_OPERATIONS:
			{
				UdpUpPlayerOperations pb_ReceiveMes = CSData.DeserializeData<UdpUpPlayerOperations>(bodyData);
				UpdatePlayerOperation (pb_ReceiveMes.operation,pb_ReceiveMes.mesID);
			}
			break;
		case CSID.UDP_UP_DELTA_FRAMES:
			{
				UdpUpDeltaFrames pb_ReceiveMes = CSData.DeserializeData<UdpUpDeltaFrames>(bodyData);

				UdpDownDeltaFrames _downData = new UdpDownDeltaFrames ();

				for (int i = 0; i < pb_ReceiveMes.frames.Count; i++) {
					int framIndex = pb_ReceiveMes.frames [i];

					UdpDownFrameOperations _downOp = new UdpDownFrameOperations ();
					_downOp.frameID = framIndex;
					_downOp.operations = dic_gameOperation [framIndex];

					_downData.framesData.Add (_downOp);
				}

				byte[] _data = CSData.GetSendMessage<UdpDownDeltaFrames> (_downData, SCID.UDP_DOWN_DELTA_FRAMES);
				dic_udp [pb_ReceiveMes.battleID].SendMessage (_data);
			}
			break;
		case CSID.UDP_UP_GAME_OVER:
			{
				UdpUpGameOver pb_ReceiveMes = CSData.DeserializeData<UdpUpGameOver>(bodyData);
				UpdatePlayerGameOver (pb_ReceiveMes.battleID);

				UdpDownGameOver _downData = new UdpDownGameOver ();
				byte[] _data = CSData.GetSendMessage<UdpDownGameOver> (_downData, SCID.UDP_DOWN_GAME_OVER);
				dic_udp [pb_ReceiveMes.battleID].SendMessage (_data);
			}
			break;
		default:
			break;
		}
	}

}
