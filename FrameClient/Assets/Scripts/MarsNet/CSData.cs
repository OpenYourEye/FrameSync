using System.IO;
using System.Collections.Generic;
using System;
//包头结构
public struct PackageConstant
{
	public static int PackMessageIdOffset = 0;
	// 消息id (1个字节)
	public static int PacklengthOffset = 1;
	//消息包长度 (2个字节)
	public static int PacketHeadLength = 3;
	//包头长度
}
	
public class CSData
{
	public static byte[] GetSendMessage<T> (T pb_Body,PBCommon.CSID messageID)
	{
		byte[] packageBody = CSData.SerializeData<T> (pb_Body);
		byte packMessageId = (byte)messageID; //消息id (1个字节)

		int packlength = PackageConstant.PacketHeadLength + packageBody.Length; //消息包长度 (2个字节)
		byte[] packlengthByte = BitConverter.GetBytes ((short)packlength);

		List<byte> packageHeadList = new List<byte> (); 
		//包头信息
		packageHeadList.Add (packMessageId);
		packageHeadList.AddRange (packlengthByte);
		//包体
		packageHeadList.AddRange (packageBody);

		return packageHeadList.ToArray ();
	}


	public static byte[] SerializeData<T> (T instance)
	{
		byte[] bytes;
		using (var ms = new MemoryStream ()) {
			ProtoBuf.Serializer.Serialize (ms, instance);
			bytes = new byte[ms.Position];
			var fullBytes = ms.GetBuffer ();
			Array.Copy (fullBytes, bytes, bytes.Length);
		}
		return bytes;
	}

	public static T DeserializeData<T> (byte[] bytes)
	{
		using (Stream ms = new MemoryStream (bytes)) {
			return ProtoBuf.Serializer.Deserialize<T> (ms);
		}
	}
}


