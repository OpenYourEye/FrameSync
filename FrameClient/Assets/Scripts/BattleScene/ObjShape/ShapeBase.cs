using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum ShapeType{
	begin,
	circle,
	rect,
	polygon,
	arc
}


public enum ObjectType{
	begin,
	role,
	bullet,
	obstacle
}

public struct ObjectUid{
	public ObjectType objectType;
	public int objectID;

	public ObjectUid(ObjectType _objectType,int _objectID){
		objectType = _objectType;
		objectID = _objectID;
	}
}

[ExecuteInEditMode()]
public class ShapeBase : MonoBehaviour {
	public GameVec2Con baseCenter;//基类圆中心_配置
	public int baseRadiusCon;//基类圆半径_配置
	[HideInInspector]
	public ShapeType type;

	private ObjectUid objUid;
	public ObjectUid ObjUid{
		get{
			return objUid;
		}
	}
		
	[HideInInspector]
	public bool isInDestory;

	protected GameVector2 basePosition;//基类圆坐标
	protected int baseRadius;//基类圆半径
	#if UNITY_EDITOR
	public bool closeLine = false;
	public Color lineColor = Color.yellow;
	public bool fixPosition = false;
	#endif

	public void InitSelf(ObjectType _objType,int _objID){
		isInDestory = false;
		objUid = new ObjectUid (_objType,_objID);

		basePosition = ToolGameVector.ChangeGameVectorConToGameVector2 (baseCenter);
		baseRadius = ToolMethod.Config2Logic(baseRadiusCon);
		InitData ();
	}

	public virtual void InitData(){

	}
		
	//获取逻辑坐标
	public virtual GameVector2 GetPosition(){
		return basePosition;
	}
	//获取渲染坐标
	public Vector3 GetPositionVec3(float _y = 0f){
		return ToolGameVector.ChangeGameVectorToVector3 (basePosition,_y);
	}
	//设置逻辑坐标
	public void SetPosition (GameVector2 _pos){
		basePosition = _pos;
	}
	//获取基类圆半径
	public virtual int GetRadius(){
		return baseRadius;
	}
	//距离检测
	public virtual bool IsInBaseCircleDistance(GameVector2 _pos,int _radius){
		return ToolGameVector.CollideCircleAndCircle (_pos, basePosition, _radius,baseRadius);
	}
	//距离检测,out距离平方
	public virtual bool IsInBaseCircleDistanceOutDistance(GameVector2 _pos,int _radius,out long _dis){
		int newRange = _radius + baseRadius;
		return ToolGameVector.IsInDistance (_pos, basePosition, newRange,out _dis);
	}
	//是否碰撞
	public virtual bool IsCollisionCircle(GameVector2 _pos,int _radius){
		return ToolGameVector.CollideCircleAndCircle (_pos, basePosition, _radius,baseRadius);
	}

	//是否碰撞,带位置修正
	public virtual bool IsCollisionCircleCorrection(GameVector2 _pos,int _radius,out GameVector2 _amend){
		return ToolGameVector.CollideCircleAndCircle (_pos,basePosition,_radius,baseRadius,out _amend);
	}
}
