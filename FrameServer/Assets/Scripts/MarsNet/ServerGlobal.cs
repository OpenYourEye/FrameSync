using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
public class ServerGlobal {


	private static ServerGlobal instance;
	private List<Action> list_action = new List<Action> ();
	private Mutex mutex_actionList = new Mutex ();

	public string serverIp;
	public static ServerGlobal Instance
	{
		get{ 
			if (instance == null)
			{
				instance = new ServerGlobal();
			}
			return instance;
		}
	}

	private ServerGlobal()
	{
		GameObject obj = new GameObject ("ServerGlobal");
		obj.AddComponent<ServerUpdate> ();
		GameObject.DontDestroyOnLoad (obj);


		serverIp = Network.player.ipAddress;
	}

	public void Destory(){
		instance = null;
	}


	public void AddAction (Action _action)
	{
		mutex_actionList.WaitOne ();
		list_action.Add (_action);
		mutex_actionList.ReleaseMutex ();
	}

	public void DoForAction ()
	{
		mutex_actionList.WaitOne ();
		for (int i = 0; i < list_action.Count; i++) {
			list_action [i] ();
		}
		list_action.Clear ();
		mutex_actionList.ReleaseMutex ();
	}

}


public class ServerUpdate : MonoBehaviour
{
	void Update ()
	{
		ServerGlobal.Instance.DoForAction ();
	}

	void OnApplicationQuit ()
	{
		LogManage.Instance.Destory ();
		ServerTcp.Instance.EndServer ();
		BattleManage.Instance.Destroy ();
	}
}
