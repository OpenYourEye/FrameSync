using UnityEngine;
using System;
using System.Text;
using System.Threading;

using System.Net;
using System.Net.Sockets;

using PBCommon;
using PBBattle;
public class MarsUdp {
	public delegate void DelegateAnalyzeMessage(SCID messageId,byte[] bodyData);
	public DelegateAnalyzeMessage delegate_analyze_message;

	private UdpClient sendClient = null;
//	private IPEndPoint sendEndPort;
	private bool isRun;

	private bool isRecv;
	public void StartClientUdp(string _ip){

//		if (sendEndPort != null) {
//			Debug.Log ("客户端udp已经启动~");
//			return;
//		}
			
		if (isRun) {
			Debug.Log ("客户端udp已经启动~");
			return;
		}
		isRun = true;

		sendClient = UdpManager.Instance.GetClient();
//		sendEndPort = new IPEndPoint(IPAddress.Parse(_ip), NetConfig.UdpSendPort);

		StartRecvMessage ();
	}
		
	private void StartRecvMessage(){
		Thread t = new Thread(new ThreadStart(RecvThread));
		t.Start();
	}

	public void StopRecvMessage(){
		isRecv = false;
	}

	public void EndClientUdp(){
		try {
			isRun = false;
			isRecv = false;
//			if (sendEndPort != null) {
//				UdpManager.Instance.CloseUdpClient();
//				sendClient = null;
//				sendEndPort = null;
//			}
			UdpManager.Instance.CloseUdpClient();
			sendClient = null;
			delegate_analyze_message = null;	
		} catch (Exception ex) {
			Debug.Log ("udp连接关闭异常:" + ex.Message);
		}

	}

	public void SendMessage(byte[] _mes){
		if (isRun) {
			try {
				sendClient.Send(_mes,_mes.Length);
//				sendClient.Send (_mes,_mes.Length,sendEndPort);	
				BattleData.Instance.sendNum+=_mes.Length;
//				Debug.Log("发送量:" + _mes.Length.ToString());
//				Debug.Log("udp发送量：" + _mes.Length);
			} catch (Exception ex) {
				Debug.Log ("udp发送失败:" + ex.Message);
			}
		}
	}


	private void RecvThread()
	{
		isRecv = true;
		IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(NetGlobal.Instance().serverIP), UdpManager.Instance.localPort);
		while (isRecv)
		{
			try {
				byte[] buf = sendClient.Receive(ref endpoint);

				byte packMessageId = buf[PackageConstant.PackMessageIdOffset];     //消息id (1个字节)
				Int16 packlength = BitConverter.ToInt16(buf,PackageConstant.PacklengthOffset);  //消息包长度 (2个字节)
				int bodyDataLenth = packlength - PackageConstant.PacketHeadLength;
				byte[] bodyData = new byte[bodyDataLenth];
				Array.Copy(buf, PackageConstant.PacketHeadLength, bodyData, 0, bodyDataLenth);

				delegate_analyze_message((SCID)packMessageId,bodyData);

				//是客户端,统计接收量
				BattleData.Instance.recvNum+=buf.Length;
//				Debug.Log("发送量:" + buf.Length.ToString() + "," + GameData.Instance().recvNum.ToString());
			} catch (Exception ex) {
				Debug.Log ("udpClient接收数据异常:" + ex.Message);
			}
		}
		Debug.Log ("udp接收线程退出~~~~~");
	}


	void OnDestroy()
	{
		EndClientUdp ();
	}
}