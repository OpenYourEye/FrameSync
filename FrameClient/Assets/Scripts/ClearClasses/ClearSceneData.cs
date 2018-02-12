using UnityEngine;
using System.Collections;
using System;
using Object = UnityEngine.Object;
using UnityEngine.SceneManagement;
public class ClearSceneData : MonoBehaviour {

	//异步对象
	private AsyncOperation async;
//	public Transform _roleParent;
//	AssetBundle pictureAB;

	private static int nextScene;


	void Start()
	{
		StartCoroutine("ClearResouces");
	}

	IEnumerator ClearResouces(){

//		int _id = 10001;
//		string abName = "playerbigpic" + _id.ToString ();
//		string path = GlobalData.GetInstance ().GetABPath (abName);
//		pictureAB = AssetBundle.LoadFromFile (path);
//		yield return pictureAB;
//
//		string objName = "role_" + _id.ToString ();
//
//		var perfreb_player = pictureAB.LoadAsset<GameObject> (objName);
//
//		var obj_player = Instantiate (perfreb_player,_roleParent) as GameObject;
//		obj_player.transform.localPosition = Vector3.zero;
//		obj_player.transform.localEulerAngles = Vector3.zero; 
//		obj_player.transform.localScale = Vector3.one;
//
//
		Resources.UnloadUnusedAssets();
		yield return new WaitForSeconds (0.1f);

//		Material[] matAry = Resources.FindObjectsOfTypeAll<Material>();
//
//		int _num = 0;
//		for (int i = 0; i < matAry.Length; ++i)
//		{
//			matAry [i] = null;
//			_num++;
//			if (_num % 5 == 0) yield return null;
//		}
//			
//		Texture[] TexAry = Resources.FindObjectsOfTypeAll<Texture>();
//
//		for (int i = 0; i < TexAry.Length; ++i)
//		{
//			TexAry [i] = null;
//			_num++;
//			if (_num % 5 == 0) yield return null;
//		}
			
		//卸载没有被引用的资源
		Resources.UnloadUnusedAssets();

		//立即进行垃圾回收
		GC.Collect();
		GC.WaitForPendingFinalizers();//挂起当前线程，直到处理终结器队列的线程清空该队列为止
		GC.Collect();


		yield return null;
		StartCoroutine("AsyncLoadScene", nextScene);
	}

	/// <summary>
	/// 静态方法，直接切换到ClearScene，此脚本是挂在ClearScene场景下的，就会实例化，执行资源回收
	/// </summary>
	/// <param name="_nextSceneName"></param>
	public static void LoadScene(int _nextScene)
	{
		nextScene = _nextScene;
		SceneManager.LoadScene (GameConfig.clearScene);  

	}

	/// <summary>
	/// 异步加载下一个场景
	/// </summary>
	/// <param name="sceneName"></param>
	/// <returns></returns>
	IEnumerator AsyncLoadScene(int scene)
	{
		async = SceneManager.LoadSceneAsync(scene);
		yield return async;
	}

	void OnDestroy()
	{
		async = null;
//		pictureAB.Unload (true);
		Resources.UnloadUnusedAssets();
	}
}
