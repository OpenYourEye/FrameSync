using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PBBattle;
public class GameCon : MonoBehaviour {

	public bool isBattle = true;

	public UIGameOver uiGameOver;
	public GameObject uiReady;
	public Transform mapTranform;
	void Start () {
		if (isBattle) {
			uiReady.SetActive (true);
			BattleCon _battleCon = gameObject.AddComponent<BattleCon> ();
			_battleCon.delegate_readyOver = ReadyFinish;
			_battleCon.delegate_gameOver = GameOver;
			_battleCon.InitData (mapTranform);			
		}
	}

	void ReadyFinish(){
		uiReady.SetActive (false);
	}

	void GameOver(){
		uiGameOver.ShowSelf ();
	}
}
