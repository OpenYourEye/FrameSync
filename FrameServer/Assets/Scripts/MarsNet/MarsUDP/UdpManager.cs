using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;

public class UdpManager {
	private static UdpManager singleInstance;
	private static readonly object padlock = new object();

	public UdpClient _udpClient = null;
	public int recvPort;
	public static UdpManager Instance
	{
		get
		{
			lock (padlock)
			{
				if (singleInstance==null)
				{
					singleInstance = new UdpManager();
				}
				return singleInstance;
			}
		}
	}

	private UdpManager()
	{
		CreatUdp ();
	}

	public void Creat(){

	}

	void CreatUdp(){
		_udpClient = new UdpClient (ServerConfig.udpRecvPort);
		IPEndPoint _localip = (IPEndPoint)_udpClient.Client.LocalEndPoint;
		Debug.Log ("udp端口:" + _localip.Port);
		recvPort = _localip.Port;
	}

	public void Destory(){

		CloseUdpClient ();
		singleInstance = null;
	}

	public void CloseUdpClient(){
		if (_udpClient != null) {
			_udpClient.Close ();
			_udpClient = null;
		}
	}

	public UdpClient GetClient(){
		if (_udpClient == null) {
			CreatUdp ();
		}
		return _udpClient;
	}


}
