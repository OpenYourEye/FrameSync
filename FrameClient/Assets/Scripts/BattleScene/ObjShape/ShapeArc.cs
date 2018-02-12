using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeArc : ShapeBase {

	public GameVec2Con arcCenterCon;
	protected GameVector2 arcCenter;
	public int arcRadiusCon;
	protected int arcRadius;

	public int arcAngle;
	public int arcAngleSize;

	#if UNITY_EDITOR
	[Header("是否显示圆弧的整圆")]
	public bool showArcCircle = true;

	#endif
	public override void InitData(){
		type = ShapeType.arc;
		arcRadius = ToolMethod.Config2Logic(arcRadiusCon);

		arcCenter = ToolGameVector.ChangeGameVectorConToGameVector2 (arcCenterCon) + basePosition;
	}
		
	public override bool IsCollisionCircle(GameVector2 _pos,int _radius){
		return ToolGameVector.CollideCircleAndArc (_pos,_radius,arcCenter,arcRadius,arcAngle,arcAngleSize);
	}

	public override bool IsCollisionCircleCorrection(GameVector2 _pos,int _radius,out GameVector2 _amend){
		return ToolGameVector.CollideCircleAndArc (_pos,_radius,arcCenter,arcRadius,arcAngle,arcAngleSize,out _amend);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (closeLine) {
			return;
		}
		Gizmos.color = lineColor;


		Vector3 baseVec3 = ToolGameVector.ChangeGameVectorConToVector3 (baseCenter);
		Gizmos.DrawWireSphere (baseVec3,baseRadiusCon);

		Vector3 arcCenter = baseVec3 + ToolGameVector.ChangeGameVectorConToVector3 (arcCenterCon);

		if (showArcCircle) {
			Gizmos.DrawWireSphere (arcCenter,arcRadiusCon);	
		}

		int vectNumber = arcAngleSize / 3;

		Vector3 firstVect1 = arcCenter + new Vector3 (Mathf.Cos(Mathf.Deg2Rad * arcAngle) * arcRadiusCon,Mathf.Sin(Mathf.Deg2Rad * arcAngle) * arcRadiusCon,0);
		Vector3 firstVect2 = firstVect1;

//		Gizmos.color = Color.blue;
		for (int i = 1; i <= vectNumber; i++) {
			int angle1 = arcAngle + 3 * i;
			int angle2 = arcAngle - 3 * i;
			if (i == vectNumber) {
				angle1 = arcAngle + arcAngleSize;
				angle2 = arcAngle - arcAngleSize;
			}

			float fAngle1 = Mathf.Deg2Rad * angle1;
			float fAngle2 = Mathf.Deg2Rad * angle2;

			Vector3 vectPos1 = arcCenter + new Vector3 (Mathf.Cos(fAngle1) * arcRadiusCon,Mathf.Sin(fAngle1) * arcRadiusCon,0);
			Gizmos.DrawLine (firstVect1,vectPos1);
			firstVect1 = vectPos1;


			Vector3 vectPos2 = arcCenter + new Vector3 (Mathf.Cos(fAngle2) * arcRadiusCon,Mathf.Sin(fAngle2) * arcRadiusCon,0);
			Gizmos.DrawLine (firstVect2,vectPos2);
			firstVect2 = vectPos2;

		}

			
		if (fixPosition) {
			int _posX = (int)transform.position.x + 25;
			int _posY = (int)transform.position.y + 25;

			_posX = _posX - _posX % 50;
			_posY = _posY - _posY % 50;


			baseCenter.x = _posX;
			baseCenter.y = _posY;	

			transform.position = new Vector3 (_posX * 1.0f,_posY * 1.0f);

			fixPosition = false;
		}

	}
	#endif
}
