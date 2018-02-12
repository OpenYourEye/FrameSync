using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeRect : ShapeBase {

	[HideInInspector]
	public int halfWidth;
	[HideInInspector]
	public int halfHeight;

	public int halfWidthCon;
	public int halfHeightCon;

//	private Rect selfRect;
	#if UNITY_EDITOR
	public bool hideCircle = false;
	public bool updateRadius = false;
	#endif
	public override void InitData(){
		type = ShapeType.rect;

		halfWidth = ToolMethod.Config2Logic(halfWidthCon);
		halfHeight = ToolMethod.Config2Logic(halfHeightCon);
	}
		
	public override bool IsCollisionCircle(GameVector2 _pos,int _radius){
		return ToolGameVector.CollideCircleAndRect (_pos,_radius,basePosition,halfWidth,halfHeight);
	}

	public override bool IsCollisionCircleCorrection(GameVector2 _pos,int _radius,out GameVector2 _amend){
		return ToolGameVector.CollideCircleAndRect (_pos, _radius, basePosition, halfWidth, halfHeight, out _amend);
	}


	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (closeLine) {
			return;
		}
			
		Vector3 _center;
		if (Application.isPlaying) {
			_center = ToolGameVector.ChangeGameVectorToVector3 (basePosition);
		} else {
			_center = ToolGameVector.ChangeGameVectorConToVector3 (baseCenter);
		}

		if (!hideCircle) {
			Gizmos.color = Color.blue;

			Gizmos.DrawWireSphere (_center,ToolMethod.Config2Render (baseRadiusCon));	
		}


		Gizmos.color = lineColor;

		float _width = ToolMethod.Config2Render (halfWidthCon);
		float _heigh = ToolMethod.Config2Render (halfHeightCon);

		Vector3[] topVec = new Vector3[4];
		topVec [0] = _center + new Vector3 (-_width,0,-_heigh);
		topVec [1] = _center + new Vector3 (-_width,0,_heigh);
		topVec [2] = _center + new Vector3 (_width,0,_heigh);
		topVec [3] = _center + new Vector3 (_width,0,-_heigh);

		Gizmos.DrawLine (topVec[0],topVec[1]);
		Gizmos.DrawLine (topVec[1],topVec[2]);
		Gizmos.DrawLine (topVec[2],topVec[3]);
		Gizmos.DrawLine (topVec[3],topVec[0]);


		if (updateRadius) {
			baseRadiusCon = (int)Mathf.Sqrt(halfWidthCon * halfWidthCon + halfHeightCon * halfHeightCon) + 1;

			updateRadius = false;
		}


		if (fixPosition) {
			int _posX = (int)ToolMethod.Render2Config(transform.position.x) + 25;
			int _posY = (int)ToolMethod.Render2Config(transform.position.z) + 25;

			_posX = _posX - _posX % 50;
			_posY = _posY - _posY % 50;

			baseCenter.x = _posX;
			baseCenter.y = _posY;	

			transform.position = ToolGameVector.ChangeGameVectorConToVector3(baseCenter);

			fixPosition = false;
		}

	}
	#endif
}
