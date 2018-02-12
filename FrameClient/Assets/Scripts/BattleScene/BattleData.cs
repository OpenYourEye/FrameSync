using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PBBattle;
public class BattleData {
	public int randSeed; //随机种子
	public int battleID;

	public const int mapRow = 7;//行数
	public const int mapColumn = 13;//列
	public const int gridLenth = 10000;//格子的逻辑大小
	public const int gridHalfLenth = 5000;//格子的逻辑大小
	public int mapTotalGrid;
	public int mapWidth;
	public int mapHeigh;

	public List<BattleUserInfo> list_battleUser;
	private Dictionary<int,GameVector2> dic_speed;

	private int curOperationID;
	public PlayerOperation selfOperation;

	private int curFramID;
	private int maxFrameID;
	private int maxSendNum;

	private List<int> lackFrame;
	private Dictionary<int,AllPlayerOperation> dic_frameDate;
	private Dictionary<int,int> dic_rightOperationID;


	//一些统计数据
	public int fps;
	public int netPack;
	public int sendNum;
	public int recvNum;

	private static BattleData instance;
	public static BattleData Instance
	{
		get	{ 
			// 如果类的实例不存在则创建，否则直接返回
			if (instance == null) {
				instance = new BattleData ();
			}
			return instance;
		}
	}

	private BattleData(){

		mapTotalGrid = mapRow * mapColumn;
		mapWidth = mapColumn * gridLenth;
		mapHeigh = mapRow * gridLenth;

		curOperationID = 1;
		selfOperation = new PlayerOperation ();
		selfOperation.move = 121;
		ResetRightOperation ();

		dic_speed = new Dictionary<int, GameVector2> ();
		//初始化速度表
		GlobalData.Instance ().GetFileStringFromStreamingAssets ("Desktopspeed.txt", _fileStr => {
			InitSpeedInfo (_fileStr);
		});

		curFramID = 0;
		maxFrameID = 0;
		maxSendNum = 5;

		lackFrame = new List<int> ();
		dic_rightOperationID = new Dictionary<int, int> ();
		dic_frameDate = new Dictionary<int, AllPlayerOperation> ();
	}

	public void UpdateBattleInfo(int _randseed,List<BattleUserInfo> _userInfo){
		randSeed = _randseed;
		list_battleUser = new List<BattleUserInfo> (_userInfo);

		foreach (var item in list_battleUser) {
			if (item.uid == NetGlobal.Instance().userUid) {
				battleID = item.battleID;
				selfOperation.battleID = battleID;
				Debug.Log ("自己的战斗id:" + battleID);
			}

			dic_rightOperationID [item.battleID] = 0;
		}
	}

	public void ClearData ()
	{
		curOperationID = 1;
		selfOperation.move = 121;
		ResetRightOperation ();

		curFramID = 0;
		maxFrameID = 0;
		maxSendNum = 5;

		lackFrame.Clear();
		dic_rightOperationID.Clear ();
		dic_frameDate.Clear();
	}


	public void Destory ()
	{
		list_battleUser.Clear ();
		list_battleUser = null;
		instance = null;
	}

	void InitSpeedInfo (string _fileStr)
	{
		string[] lineArray = _fileStr.Split ("\n" [0]); 

		int dir;
		for (int i = 0; i < lineArray.Length; i++) {
			if (lineArray [i] != "") {
				GameVector2 date = new GameVector2 ();
				string[] line = lineArray [i].Split (new char[1]{ ',' }, 3);
				dir = System.Int32.Parse (line [0]);
				date.x = System.Int32.Parse (line [1]);
				date.y = System.Int32.Parse (line [2]);
				dic_speed [dir] = date;
			}
		}
	}

	public GameVector2 GetSpeed (int _dir)
	{
		return dic_speed [_dir];
	}
	//坐标不超出地图
	public GameVector2 GetMapLogicPosition(GameVector2 _pos){
		return new GameVector2 (Mathf.Clamp(_pos.x,0,mapWidth),Mathf.Clamp(_pos.y,0,mapHeigh));
	}

	public GameVector2 GetMapGridCenterPosition(int _row,int _column){
		return new GameVector2 (_column * gridLenth + gridHalfLenth,_row * gridLenth + gridHalfLenth);
	}

	public GameVector2 GetMapGridFromRand(int _randNum){
		int _num1 = _randNum % mapTotalGrid;
		int _row = _num1 / mapColumn;
		int _column = _num1 % mapColumn;
		return new GameVector2 (_row, _column);
	}

	public GameVector2 GetMapGridCenterPositionFromRand(int _randNum){
		GameVector2 grid = GetMapGridFromRand (_randNum);
		return GetMapGridCenterPosition (grid.x, grid.y);
	}


	public void UpdateMoveDir (int _dir)
	{
		selfOperation.move = _dir;
	}

	public void UpdateRightOperation(RightOpType _type,int _value1,int _value2){
		selfOperation.rightOperation = _type;
		selfOperation.operationValue1 = _value1;
		selfOperation.operationValue2 = _value2;
		selfOperation.operationID = curOperationID;
	}

	public bool IsValidRightOp(int _battleID,int _rightOpID){
		return _rightOpID > dic_rightOperationID [_battleID];
	}

	public void UpdateRightOperationID(int _battleID,int _opID,RightOpType _type){
		dic_rightOperationID [_battleID] = _opID;
		if (battleID == _battleID) {
			//玩家自己
			curOperationID++;
			if (_type == selfOperation.rightOperation) {
				ResetRightOperation ();
			}
		}
	}

	public void ResetRightOperation(){
		selfOperation.rightOperation = RightOpType.noop;
		selfOperation.operationValue1 = 0;
		selfOperation.operationValue2 = 0;
		selfOperation.operationID = 0;
	}

	public int GetFrameDataNum(){
		if (dic_frameDate == null) {
			return 0;
		} else {
			return dic_frameDate.Count;
		}
	}

	public void AddNewFrameData(int _frameID,AllPlayerOperation _op){
		dic_frameDate [_frameID] = _op;
		for (int i = maxFrameID + 1; i < _frameID; i++) {
			lackFrame.Add (i);
			Debug.Log ("缺失 :" + i);
		}
		maxFrameID = _frameID;

		//发送缺失帧数据
		if (lackFrame.Count > 0) {
			if (lackFrame.Count > maxSendNum) {
				List<int> sendList = lackFrame.GetRange (0, maxSendNum);
				UdpPB.Instance ().SendDeltaFrames (selfOperation.battleID,sendList);
			} else {
				UdpPB.Instance ().SendDeltaFrames (selfOperation.battleID,lackFrame);
			}
		}
	}

	public void AddLackFrameData (int _frameID, AllPlayerOperation _newOp)
	{
		//删除缺失的帧记录
		if (lackFrame.Contains(_frameID)) {
			dic_frameDate [_frameID] = _newOp;
			lackFrame.Remove (_frameID);
			Debug.Log ("补上 :" + _frameID);
		}
	}

	public bool TryGetNextPlayerOp (out AllPlayerOperation _op)
	{
		int _frameID = curFramID + 1;	
		return dic_frameDate.TryGetValue (_frameID,out _op);
	}

	public void RunOpSucces ()
	{
		curFramID++;
	}
}
