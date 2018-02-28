using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogManage {

	public delegate void DelegateLogChange(string _str);
	public DelegateLogChange logChange;

	private static LogManage instance = null;
	public static LogManage Instance
	{
		get{ 
			if (instance == null) {
				instance = new LogManage ();
			}
			return instance;
		}
	}

	private LogManage(){
		
	}
	public void Destory(){
		logChange = null;
		instance = null;
	}

	public void AddLog(string _str){
		if (logChange != null) {
			logChange (_str);
		}
	}
}
