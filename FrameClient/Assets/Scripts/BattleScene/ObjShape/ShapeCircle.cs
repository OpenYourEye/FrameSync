using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeCircle : ShapeBase {

	public override void InitData(){
		type = ShapeType.circle;
	}
		
	#if UNITY_EDITOR
	void OnDrawGizmos()
	{
		if (closeLine) {
			return;
		}
		Gizmos.color = lineColor;

		if (Application.isPlaying) {
			Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorToVector3(basePosition),ToolMethod.Config2Render(baseRadiusCon));
		} else {
			Gizmos.DrawWireSphere (ToolGameVector.ChangeGameVectorConToVector3(baseCenter),ToolMethod.Config2Render(baseRadiusCon));
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
