using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ServerTcp {
 
	static Socket serverSocket;  

	private bool isRun = false;
	private Dictionary<string,Socket> dic_clientSocket = new Dictionary<string, Socket>();


	private static readonly object stLockObj = new object ();
	private static ServerTcp instance; 
	public static ServerTcp Instance
	{
		get{ 
			lock (stLockObj) {
				if (instance == null)
				{
					instance = new ServerTcp();
				}	
			}
			return instance;
		}
	}

	private ServerTcp()
	{
		
	}

	public void Destory(){
		instance = null;
	}

	public void StartServer(){

		try {
			IPAddress ip = IPAddress.Parse(ServerGlobal.Instance.serverIp);

			serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);  

			serverSocket.Bind(new IPEndPoint(ip, ServerConfig.servePort));  //绑定IP地址：端口  
			serverSocket.Listen(20);    //设定最多10个排队连接请求  
			Debug.Log("启动监听" + serverSocket.LocalEndPoint.ToString() + "成功");  
			isRun = true;

			//通过Clientsoket发送数据  
			Thread myThread = new Thread(ListenClientConnect);  
			myThread.Start();  	

		} catch (Exception ex) {
			Debug.Log ("服务器启动失败:" + ex.Message);
		}    
	}



	private void ListenClientConnect()  
	{  
		while (isRun)  
		{  
			try {
				Socket clientSocket = serverSocket.Accept();   

//				clientSocket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);

				Thread receiveThread = new Thread(ReceiveMessage);  
				receiveThread.Start(clientSocket);  	
			} catch (Exception ex) {
				Debug.Log ("监听失败:" + ex.Message);
			}
		}  
	}  


	public void EndServer(){

		if (!isRun) {
			return;
		}

		isRun = false;
		try {
			foreach (var item in dic_clientSocket) {
				item.Value.Close ();
			}

			dic_clientSocket.Clear ();

			if (serverSocket != null) {
				serverSocket.Close ();
				serverSocket = null;	
			}	
		} catch (Exception ex) {
			Debug.Log ("tcp服务器关闭失败:" + ex.Message);
		}

	}

	public void CloseClientTcp(string _socketIp){
		try {
			if (dic_clientSocket.ContainsKey(_socketIp)) {
				if (dic_clientSocket [_socketIp] != null) {
					dic_clientSocket [_socketIp].Close();
				}
				dic_clientSocket.Remove (_socketIp);
			}	
		} catch (Exception ex) {
			Debug.Log ("关闭客户端..." + ex.Message);
		}

	}

	public int GetClientCount(){
		return dic_clientSocket.Count;
	}

	public List<string> GetAllClientIp(){
		return new List<string> (dic_clientSocket.Keys);
	}

		 
	private void ReceiveMessage(object clientSocket)  
	{  
		Socket myClientSocket = (Socket)clientSocket;  
		string _socketIp = myClientSocket.RemoteEndPoint.ToString().Split(':')[0]; 

		Debug.Log ("有客户端连接:" + _socketIp);

		dic_clientSocket[_socketIp] = myClientSocket;	

		bool _flag = true;

		byte[] resultData = new byte[1024];
		while (isRun && _flag)  
		{  
			try  
			{  
//				Debug.Log("_socketName是否连接:" + myClientSocket.Connected);
				//通过clientSocket接收数据  
				if (myClientSocket.Poll(1000,SelectMode.SelectRead)) {
					throw new Exception("客户端关闭了1~");
				}

				int _size = myClientSocket.Receive(resultData);  

				if (_size <= 0) {
					throw new Exception("客户端关闭了2~");
				}

				byte packMessageId = resultData[PackageConstant.PackMessageIdOffset];     //消息id (1个字节)
				Int16 packlength = BitConverter.ToInt16(resultData,PackageConstant.PacklengthOffset);  //消息包长度 (2个字节)
				int bodyDataLenth = packlength - PackageConstant.PacketHeadLength;
				byte[] bodyData = new byte[bodyDataLenth];
				Array.Copy(resultData, PackageConstant.PacketHeadLength, bodyData, 0, bodyDataLenth);


				TcpPB.Instance().AnalyzeMessage((PBCommon.CSID)packMessageId,bodyData,_socketIp);

			}  
			catch(Exception ex)  
			{  
				Debug.Log("接收客户端数据异常:" + ex.Message);  

				_flag = false;
				break;  
			}  
		}  
			
		CloseClientTcp (_socketIp);
	}  

	public void SendMessageAll(byte[] _mes){
		if (isRun) {
			try {
				foreach (var item in dic_clientSocket) {
					item.Value.Send (_mes);
				}	
			} catch (Exception ex) {
				Debug.Log ("发数据给所有人异常:" + ex.Message);
			}	
		}
	}
		
	public void SendMessage(string _socketName,byte[] _mes){
		if (isRun) {
			try {
				dic_clientSocket [_socketName].Send (_mes);	
			} catch (Exception ex) {
				Debug.Log ("发数据给异常:" + ex.Message);
			}	
		}

	}
}
