using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//using System.Threading;
using System.IO;

public class GlobalData
{
	private static GlobalData instance;
	//StreamingAssets文件夹路径
	public string m_sStreamingAssetsPath;
	//assetbundle对应的平台后缀
	public string m_strABExtra;
	private ClassForUpdate classForUpdate;

	public static GlobalData Instance ()
	{
		if (instance == null) {
			instance = new GlobalData ();
		}
		return instance;
	}

	public void Destory ()
	{
		instance = null;
	}

	public GlobalData ()
	{
		GameObject obj = new GameObject ("GlobalObj");
		classForUpdate = obj.AddComponent<ClassForUpdate> ();
		GameObject.DontDestroyOnLoad (obj);

		#if UNITY_ANDROID  && !UNITY_EDITOR
		m_sStreamingAssetsPath = "jar:file://" + Application.dataPath + "!/assets";   
		#elif UNITY_IPHONE && !UNITY_EDITOR
		m_sStreamingAssetsPath = Application.streamingAssetsPath;
		#else  
		m_sStreamingAssetsPath = Application.streamingAssetsPath;
		#endif 

		#if UNITY_ANDROID 
		m_strABExtra = "_android";   
		#elif UNITY_IOS 
		m_strABExtra = "_ios";  
		#elif UNITY_STANDALONE_OSX 
		m_strABExtra = "_mac"; 
		#elif UNITY_STANDALONE_WIN 
		m_strABExtra = "_win"; 
		#else  
		Debug.Log("没有ab文件的平台啊～～～～");
		#endif 
	}

	public void SetScreenResolution (int _width, int _heigh)
	{
//		#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
//		Screen.SetResolution (_width,_heigh,true);
//		scaleScreen =  originScreenSize.y / _heigh;
//		#endif  
	}

	public bool IsChinese ()
	{
		return Application.systemLanguage == SystemLanguage.Chinese;
	}

	public string GetABPath (string _file)
	{
		return m_sStreamingAssetsPath + "/AssetBundle/" + _file + m_strABExtra;
	}

	//读取StreamingAssets下的文件
	public void GetFileStringFromStreamingAssets (string _fileName, Action<string> _action)
	{
		string fullPath = m_sStreamingAssetsPath + "/" + _fileName;
		classForUpdate.GetFileStringFromStreamingAssets (fullPath, _action);
	}
 
}

public class ClassForUpdate : MonoBehaviour
{
	public void GetFileStringFromStreamingAssets (string _fileName, Action<string> _action)
	{
		#if UNITY_ANDROID  && !UNITY_EDITOR
		StartCoroutine (GetFileStringForAndroid(_fileName,_action));
		#elif UNITY_IPHONE && !UNITY_EDITOR
		GetFileStringForIos(_fileName,_action);
		#else  
		GetFileString (_fileName, _action);
		#endif 
	}

	IEnumerator GetFileStringForAndroid (string _path, Action<string> _action)
	{ 
		WWW wwwCar = new WWW (_path);  
		yield return wwwCar;  
		_action (wwwCar.text);
	}

	private void GetFileStringForIos (string _path, Action<string> _action)
	{  
		if (File.Exists (_path)) { 
			try {  
				StreamReader sr = File.OpenText (_path); 
				_action (sr.ReadToEnd ());
				sr.Close (); 
				sr.Dispose ();
			} catch {
				Debug.Log ("_path_car出错咯～");
			} 
		} 
	}

	private void GetFileString (string _path, Action<string> _action)
	{
		if (File.Exists (_path)) {
			try {  
				//实例化文件流，参数1 路径，参数2文件操作方式  
				FileStream file = new FileStream (_path, FileMode.Open);  
				StreamReader sr = new StreamReader (file);  
				_action (sr.ReadToEnd ());
				sr.Close ();  //关闭流释放空间  
				file.Close (); 
				sr.Dispose ();
			} catch {
				Debug.Log ("文件出错咯:" + _path);
			} 
		} 
	}
}
