using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICon : MonoBehaviour {

	public Text serverIP;
	// Use this for initialization
	void Start () {
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		serverIP.text = ServerGlobal.Instance.serverIp;

		ServerTcp.Instance.StartServer ();
		UdpManager.Instance.Creat ();
	}
	
	// Update is called once per frame
//	void Update () {
//		
//	}
}
