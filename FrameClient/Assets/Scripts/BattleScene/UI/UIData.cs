using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIData : MonoBehaviour {

	public Text fpsText;
	public Text netText;
	public Text timeText;
	public Text frameText;
	public Text sendText;
	public Text recvText;
	// Use this for initialization
	void Start () {
		this.InvokeRepeating ("UpdateNetInfo",1f,1f);
	}
	
	// Update is called once per frame
	void Update () {
		BattleData.Instance.fps++;
	}

	private void UpdateNetInfo(){
		if (BattleData.Instance.netPack == 0) {
			netText.text = "1000ms";
		} else {
			int _nt = (int)(1000f/BattleData.Instance.netPack);
			netText.text = _nt.ToString() + "ms";
		}
		BattleData.Instance.netPack = 0;

		int _frameNum = BattleData.Instance.GetFrameDataNum ();
		int time = _frameNum * NetConfig.frameTime /1000;

		string _timeStr = string.Format ("{0}:{1}",time/60,time%60);
		timeText.text = _timeStr;

		frameText.text ="f:" + _frameNum.ToString();

		sendText.text = "s:" + GetNumberString(BattleData.Instance.sendNum);
		recvText.text = "r:" + GetNumberString(BattleData.Instance.recvNum);


		fpsText.text = "fps:" + BattleData.Instance.fps.ToString ();
		BattleData.Instance.fps = 0;
	}

	private string GetNumberString(int _number){
		if (_number < 1024) {
			return _number.ToString () + "B";
		} else {
			float _num1 = _number / 1024f;
			if (_num1 < 1024f) {
				return string.Format ("{0:F1}K",_num1);
			} else {
				float _num2 = _num1 / 1024f;
				return string.Format ("{0:F1}M",_num2);
			}
		}
	}

	void GameOver(){
		this.CancelInvoke ();
	}
}
