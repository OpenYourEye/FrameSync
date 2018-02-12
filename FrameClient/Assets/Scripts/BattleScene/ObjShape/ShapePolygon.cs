using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapePolygon : ShapeBase {

	public GameVec2Con[] vertexsCon;
	public int[] normalDir;

	public int colliRadiusCon;//碰撞圆_配置
	protected int colliRadius;//碰撞圆半径
	#if UNITY_EDITOR
	public bool hideCircle = false;
	public bool updateVertexs = false;
	[Header("一定要按顺时针来配置顶点")]
	public GameVec2Con[] vertexs_delta;
	#endif


	public override void InitData(){
		type = ShapeType.polygon;
		colliRadius = ToolMethod.Config2Logic(colliRadiusCon);
	}

	public override bool IsCollisionCircle(GameVector2 _pos,int _radius){
		return ToolGameVector.CollideCircleAndPolygon (_pos,_radius,vertexsCon,normalDir);
	}

	public override bool IsCollisionCircleCorrection(GameVector2 _pos,int _radius,out GameVector2 _amend){
		return ToolGameVector.CollideCircleAndPolygon (_pos,_radius,vertexsCon,normalDir,out _amend);
	}

	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (closeLine) {
			return;
		}

		if (!hideCircle) {
			Gizmos.color = Color.blue;
			if (Application.isPlaying) {
				Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorToVector3(basePosition),baseRadiusCon);

				Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorToVector3(basePosition),colliRadiusCon);
			} else {
				Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorConToVector3(baseCenter),baseRadiusCon);

				Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorConToVector3(baseCenter),colliRadiusCon);
			}
		}


		if (updateVertexs || fixPosition) {
			vertexsCon = new GameVec2Con[vertexs_delta.Length];
			normalDir = new int[vertexs_delta.Length];
			float radius = 0;
			for (int i = 0; i < vertexs_delta.Length; i++) {
				vertexsCon [i].x = baseCenter.x + vertexs_delta [i].x;
				vertexsCon [i].y = baseCenter.y + vertexs_delta [i].y;

				Vector3 _dv = ToolGameVector.ChangeGameVectorConToVector3 (vertexs_delta [i]);
				radius = Mathf.Max(radius,_dv.magnitude);

				//法向
				Vector3 _dir;
				if (i == vertexs_delta.Length - 1) {
					_dir = ToolGameVector.ChangeGameVectorConToVector3 (vertexs_delta [0]);
				} else {
					_dir = ToolGameVector.ChangeGameVectorConToVector3 (vertexs_delta [i + 1]);
				}


				Vector3 _line = _dir - _dv;
				int _linedir = (int) Mathf.Sign(_line.y) * Mathf.RoundToInt(Vector3.Angle (_line,new Vector3(1f,0f,0f)));
				normalDir [i] = (int)Mathf.Repeat (_linedir + 90, 360);

			}
				
			baseRadiusCon = (int)radius + 1;
		}




		Gizmos.color = lineColor;

		for (int i = 0; i < vertexsCon.Length; i++) {

			Vector3 lineBegin;
			Vector3 lineEnd;
			if (i == vertexsCon.Length - 1) {
				lineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vertexsCon [i]);
				lineEnd = ToolGameVector.ChangeGameVectorConToVector3 (vertexsCon [0]);
			} else {
				lineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vertexsCon [i]);
				lineEnd = ToolGameVector.ChangeGameVectorConToVector3 (vertexsCon [i + 1]);
			}

			Gizmos.DrawLine (lineBegin,lineEnd);


			Vector3 _normalBegin = (lineBegin + lineEnd) * 0.5f;
			Vector3 _normalEnd = _normalBegin + new Vector3 (100f * Mathf.Cos(Mathf.Deg2Rad * normalDir [i]),100f * Mathf.Sin(Mathf.Deg2Rad * normalDir [i]));

			Gizmos.DrawLine (_normalBegin,_normalEnd);
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
