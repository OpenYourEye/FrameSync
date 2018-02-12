using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBBattle;
public class BattleCon : MonoBehaviour {

	public delegate void DelegateEvent ();
	public DelegateEvent delegate_readyOver;
	public DelegateEvent delegate_gameOver;

	private bool isBattleStart;
	private bool isBattleFinish;

	[HideInInspector]
	public RoleManage roleManage;
	[HideInInspector]
	public ObstacleManage obstacleManage;
	[HideInInspector]
	public BulletManage bulletManage;

	private static BattleCon instance;
	public static BattleCon Instance {
		get { 
			return instance;
		}
	}
	void Awake ()
	{
		instance = this;
	}

	void Start () {
		UdpPB.Instance ().StartClientUdp ();
		UdpPB.Instance ().mes_battle_start = Message_Battle_Start;
		UdpPB.Instance ().mes_frame_operation = Message_Frame_Operation;
		UdpPB.Instance ().mes_delta_frame_data = Message_Delta_Frame_Data;
		UdpPB.Instance ().mes_down_game_over = Message_Down_Game_Over;

		isBattleStart = false;
		StartCoroutine ("WaitInitData");
	}

	IEnumerator WaitInitData(){
		yield return new WaitUntil (()=>{
			return roleManage.initFinish && obstacleManage.initFinish && bulletManage.initFinish;
		});
		this.InvokeRepeating ("Send_BattleReady", 0.5f, 0.2f);
	}

	public void InitData(Transform _map){
		ToolRandom.srand ((ulong)BattleData.Instance.randSeed);
		roleManage = gameObject.AddComponent<RoleManage> ();
		obstacleManage = gameObject.AddComponent<ObstacleManage> ();
		bulletManage = gameObject.AddComponent<BulletManage> ();

		GameVector2[] roleGrid;
		roleManage.InitData (_map.Find("Role"),out roleGrid);
		obstacleManage.InitData (_map.Find("Obstacle"),roleGrid);
		bulletManage.InitData (_map.Find("Bullet"));
	}
		
	void Send_BattleReady(){
		UdpPB.Instance ().SendBattleReady (NetGlobal.Instance().userUid,BattleData.Instance.battleID);
	}

	void Message_Battle_Start(UdpBattleStart _mes){
		BattleStart ();
	} 

	void BattleStart ()
	{
		if (isBattleStart) {
			return;
		}
	
		isBattleStart = true;
		this.CancelInvoke ("Send_BattleReady");

		float _time = NetConfig.frameTime * 0.001f;
		this.InvokeRepeating ("Send_operation", _time, _time);

		StartCoroutine ("WaitForFirstMessage");
	}

	void Send_operation ()
	{
		UdpPB.Instance ().SendOperation ();
	}

	IEnumerator WaitForFirstMessage(){
		yield return new WaitUntil (()=>{
			return BattleData.Instance.GetFrameDataNum() > 0;
		});
		this.InvokeRepeating ("LogicUpdate",0f,0.020f);

		if (delegate_readyOver != null) {
			delegate_readyOver ();	
		}
	}

	void Message_Frame_Operation (UdpDownFrameOperations _mes)
	{
		BattleData.Instance.AddNewFrameData (_mes.frameID, _mes.operations);
		BattleData.Instance.netPack++;
	}

	//逻辑帧更新
	void LogicUpdate(){
		AllPlayerOperation _op;
		if (BattleData.Instance.TryGetNextPlayerOp(out _op)) {

			roleManage.Logic_Operation (_op);
			roleManage.Logic_Move ();
			bulletManage.Logic_Move ();
			bulletManage.Logic_Collision ();
			roleManage.Logic_Move_Correction ();
			obstacleManage.Logic_Destory ();
			bulletManage.Logic_Destory ();
			BattleData.Instance.RunOpSucces ();
		}
	}

	void Message_Delta_Frame_Data (UdpDownDeltaFrames _mes)
	{
		if (_mes.framesData.Count > 0) {
			foreach (var item in _mes.framesData) {
				BattleData.Instance.AddLackFrameData (item.frameID, item.operations);
			}
		}
	}

	public void OnClickGameOver(){
		BeginGameOver ();
	}

	void BeginGameOver ()
	{
		this.CancelInvoke ("Send_operation");
		this.InvokeRepeating ("SendGameOver", 0f, 0.5f);
	}

	void SendGameOver ()
	{
		UdpPB.Instance ().SendGameOver (BattleData.Instance.battleID);
	}

	void Message_Down_Game_Over (UdpDownGameOver _mes)
	{
		this.CancelInvoke ("SendGameOver");
		Debug.Log ("游戏结束咯～～～～～～");
		if (delegate_gameOver != null) {
			delegate_gameOver ();
		}
	}


	void OnDestroy ()
	{
		BattleData.Instance.ClearData ();
		UdpPB.Instance ().Destory ();
		instance = null;
	}
}
