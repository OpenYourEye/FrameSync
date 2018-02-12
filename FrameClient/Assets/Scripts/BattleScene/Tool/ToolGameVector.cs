using UnityEngine;
using System.Collections;

public class ToolGameVector {

	public static bool IsInDistance(GameVector2 _pos1,GameVector2 _pos2,long _distance){
		return _pos1.sqrMagnitude (_pos2) <= _distance * _distance;
	} 

	public static bool IsInDistance(GameVector2 _pos1,GameVector2 _pos2,long _distance,out long _realdis){
		_realdis = _pos1.sqrMagnitude (_pos2);
		return _realdis <= _distance * _distance;
	} 

	public static Vector3 ChangeGameVectorToVector3(GameVector2 _vec2,float _y = 0f){
		return new Vector3 (ToolMethod.Logic2Render(_vec2.x),_y,ToolMethod.Logic2Render(_vec2.y));
	}

	public static GameVector2 ChangeGameVectorConToGameVector2(GameVec2Con _vec2){
		return new GameVector2 (ToolMethod.Config2Logic(_vec2.x),ToolMethod.Config2Logic(_vec2.y));
	}

	public static Vector3 ChangeGameVectorConToVector3(GameVec2Con _vec2){
		return new Vector3 (ToolMethod.Config2Render(_vec2.x),0f,ToolMethod.Config2Render(_vec2.y));
	}

	/// <summary>
	/// 把坐标往绝对值大取整到百位,主要用于碰撞后的位置修正
	/// </summary>
	public static void RoundGameVector2(ref GameVector2 _vec2){
		_vec2.x = ToolGameVector.RoundInt(_vec2.x);
		_vec2.y = ToolGameVector.RoundInt(_vec2.y);
	}

	public static int RoundInt(int _int){
		return _int - _int % ToolMethod.Config2LogicScale + System.Math.Sign (_int) * ToolMethod.Config2LogicScale;
	}
		
	//点和圆碰撞
	public static bool CollidePointAndCircle(GameVector2 _p,GameVector2 _c,int _radius){
		long sprRadius = _radius * _radius;
		long sqrDistance = _c.sqrMagnitude (_p);
		return sqrDistance < sprRadius;
	}
	/// <summary>
	///	点和圆的碰撞,圆不动修正点的位置
	/// </summary>
	/// <returns><c>true</c>, if point and circle was collided, <c>false</c> otherwise.</returns>
	/// <param name="_p">碰撞点.</param>
	/// <param name="_c">碰撞圆心.</param>
	/// <param name="_radius">圆半径.</param>
	/// <param name="_amend">修正值.</param>
	public static bool CollidePointAndCircle(GameVector2 _p,GameVector2 _c,int _radius,out GameVector2 _amend){
		_amend = GameVector2.zero;

		long sprRadius = _radius * _radius;

		long sqrDistance = _c.sqrMagnitude (_p);

		if (sqrDistance >= sprRadius) {
			return false;
		}

		int distance = (int)Mathf.Sqrt (sqrDistance);
		int _angle = (_p - _c).Angle ();
		int amendDis = 1 + (_radius - distance)/100;
		_amend = amendDis * BattleData.Instance.GetSpeed(_angle);
		return true;
	}

	//圆和圆碰撞
	public static bool CollideCircleAndCircle(GameVector2 _c1,GameVector2 _c2,int _radius1,int _radius2){
		int sumRadius = _radius1 + _radius2;
		long sprRadius = sumRadius * sumRadius;
		long sqrDistance = _c1.sqrMagnitude (_c2);
		return sqrDistance < sprRadius;
	}
	/// <summary>
	/// 圆和圆的碰撞,圆2不动,修正是的是圆1的位置
	/// </summary>
	/// <returns><c>true</c>, if circle and circle was collided, <c>false</c> otherwise.</returns>
	/// <param name="_c1">圆1的圆心.</param>
	/// <param name="_c2">圆2的圆心.</param>
	/// <param name="_radius1">圆1的半径.</param>
	/// <param name="_radius2">圆2的半径.</param>
	/// <param name="_amend">修正值.</param>
	public static bool CollideCircleAndCircle(GameVector2 _c1,GameVector2 _c2,int _radius1,int _radius2,out GameVector2 _amend){
		return ToolGameVector.CollidePointAndCircle (_c1,_c2,(_radius1 + _radius2),out _amend);
	}
	/// <summary>
	/// 圆和圆的碰撞,2个圆的位置都修正
	/// </summary>
	/// <returns><c>true</c>, if circle and circle was collided, <c>false</c> otherwise.</returns>
	/// <param name="_c1">圆1的圆心.</param>
	/// <param name="_c2">圆2的圆心.</param>
	/// <param name="_radius1">圆1的半径.</param>
	/// <param name="_radius2">圆2的半径.</param>
	/// <param name="_amend1">圆1的修正值.</param>
	/// <param name="_amend2">圆2的修正值.</param>
	public static bool CollideCircleAndCircle(GameVector2 _c1,GameVector2 _c2,int _radius1,int _radius2,out GameVector2 _amend1,out GameVector2 _amend2){
		_amend1 = GameVector2.zero;
		_amend2 = GameVector2.zero;

		int radiusSum = _radius1 + _radius2;
		long sprRadius = radiusSum * radiusSum;

		long sqrDistance = _c1.sqrMagnitude (_c2);

		if (sqrDistance >= sprRadius) {
			return false;
		}

		int distance = (int)Mathf.Sqrt (sqrDistance);
		int _angle = (_c1 - _c2).Angle ();
		int amendDis = 1 + (int)((radiusSum - distance) * 0.5)/100;

		_amend1 = amendDis * BattleData.Instance.GetSpeed (_angle);
		_amend2 = -1 * _amend1;

		return true;
	}

	//圆和矩形碰撞
	public static bool CollideCircleAndRect(GameVector2 _c1,int _radius1,GameVector2 _rCenter,int _half_w,int _half_h){
		if (_radius1 <= 100) {
			//半径足够小，当成点来处理
			if (_c1.x <= _rCenter.x - _half_w) {
				return false;
			}

			if (_c1.y <= _rCenter.y - _half_h) {
				return false;
			}
			if (_c1.x >= _rCenter.x + _half_w) {
				return false;
			}
			if (_c1.y >= _rCenter.y + _half_h) {
				return false;
			}
			return true;
		}

		int llx = _rCenter.x - _half_w - _radius1;
		if (_c1.x <= llx) {
			return false;
		}

		int bby = _rCenter.y - _half_h - _radius1;
		if (_c1.y <= bby) {
			return false;
		}

		int rrx = _rCenter.x + _half_w + _radius1;
		if (_c1.x >= rrx) {
			return false;
		}

		int tty = _rCenter.y + _half_h + _radius1;
		if (_c1.y >= tty) {
			return false;
		}

		int lx = _rCenter.x - _half_w;
		int by = _rCenter.y - _half_h;
		int rx = _rCenter.x + _half_w;
		int ty = _rCenter.y + _half_h;

		if (_c1.x <= lx) {
			if (_c1.y > ty) {
				//左上角的顶点矩形里
				GameVector2 _ltPoint = new GameVector2(lx,ty);
				return ToolGameVector.CollidePointAndCircle (_c1, _ltPoint, _radius1);
			} else if (_c1.y < by) {
				//左下角的顶点矩形里
				GameVector2 _lbPoint = new GameVector2(lx,by);
				return ToolGameVector.CollidePointAndCircle (_c1, _lbPoint, _radius1);
			} else {
				return true;
			}
		} else if (_c1.x >= rx) {
			if (_c1.y > ty) {
				//右上角的顶点矩形里
				GameVector2 _rtPoint = new GameVector2(rx,ty);
				return ToolGameVector.CollidePointAndCircle (_c1, _rtPoint, _radius1);
			} else if (_c1.y < by) {
				//右下角的顶点矩形里
				GameVector2 _rbPoint = new GameVector2(rx,by);
				return ToolGameVector.CollidePointAndCircle (_c1, _rbPoint, _radius1);
			} else {
				return true;
			}
		} 
		return true;
	}
	/// <summary>
	/// 圆和矩形的修正碰撞
	/// </summary>
	/// <returns><c>true</c>, 产生碰撞, <c>false</c> 没有碰撞.</returns>
	/// <param name="_c1">圆心.</param>
	/// <param name="_radius1">圆半径.</param>
	/// <param name="_rCenter">矩形中心.</param>
	/// <param name="_half_w">矩形宽度的一半.</param>
	/// <param name="_half_h">矩形高度的一半.</param>
	/// <param name="_amend">修正值.</param>
	public static bool CollideCircleAndRect(GameVector2 _c1,int _radius1,GameVector2 _rCenter,int _half_w,int _half_h,out GameVector2 _amend){
		/*
			*l:左
			*r:右
			*t:上/顶
			*b:下/底
		*/
		int llx = _rCenter.x - _half_w - _radius1;
		if (_c1.x <= llx) {
			_amend = GameVector2.zero;
			return false;
		}
			
		int bby = _rCenter.y - _half_h - _radius1;
		if (_c1.y <= bby) {
			_amend = GameVector2.zero;
			return false;
		}
			
		int rrx = _rCenter.x + _half_w + _radius1;
		if (_c1.x >= rrx) {
			_amend = GameVector2.zero;
			return false;
		}
			
		int tty = _rCenter.y + _half_h + _radius1;
		if (_c1.y >= tty) {
			_amend = GameVector2.zero;
			return false;
		}

		int lx = _rCenter.x - _half_w;
		int by = _rCenter.y - _half_h;
		int rx = _rCenter.x + _half_w;
		int ty = _rCenter.y + _half_h;

		if (_c1.x <= lx) {
			if (_c1.y > ty) {
				//左上角的顶点矩形里
				GameVector2 _ltPoint = new GameVector2(lx,ty);
				return ToolGameVector.CollidePointAndCircle (_c1, _ltPoint, _radius1, out _amend);
			} else if (_c1.y < by) {
				//左下角的顶点矩形里
				GameVector2 _lbPoint = new GameVector2(lx,by);
				return ToolGameVector.CollidePointAndCircle (_c1, _lbPoint, _radius1, out _amend);
			} else {
				//左侧矩形
				_amend = new GameVector2(llx - _c1.x,0);
			}
		} else if (_c1.x >= rx) {
			if (_c1.y > ty) {
				//右上角的顶点矩形里
				GameVector2 _rtPoint = new GameVector2(rx,ty);
				return ToolGameVector.CollidePointAndCircle (_c1, _rtPoint, _radius1, out _amend);
			} else if (_c1.y < by) {
				//右下角的顶点矩形里
				GameVector2 _rbPoint = new GameVector2(rx,by);
				return ToolGameVector.CollidePointAndCircle (_c1, _rbPoint, _radius1, out _amend);
			} else {
				//右侧矩形
				_amend = new GameVector2(rrx - _c1.x,0);
			}
		} else {
			if (_c1.y > ty) {
				//中上矩形
				_amend = new GameVector2(0,tty - _c1.y);
			} else if (_c1.y < by) {
				//中下矩形
				_amend = new GameVector2(0,bby - _c1.y);
			} else {
				//矩形内
				GameVector2 _rtPoint = new GameVector2(rx,ty);
				int _rtAngle = (_rtPoint - _rCenter).Angle ();
				int _ltAngle = 180 - _rtAngle;
				int _lbAngle = 180 + _rtAngle;
				int _rbAngel = 360 - _rtAngle;

				int circleAngle = (_c1 - _rCenter).Angle ();//圆心和矩形中心的角度

				if (circleAngle > _rbAngel) {
					_amend = new GameVector2 (rrx - _c1.x, 0);
				} else if (circleAngle > _lbAngle) {
					_amend = new GameVector2 (0, bby - _c1.y);
				} else if (circleAngle > _ltAngle) {
					_amend = new GameVector2 (llx - _c1.x, 0);
				} else if (circleAngle > _rtAngle) {
					_amend = new GameVector2 (0, tty - _c1.y);
				} else {
					_amend = new GameVector2 (rrx - _c1.x, 0);
				}
			}
		}

		return true;
	}

	/// <summary>
	/// 点到直线距离
	/// </summary>
	/// <param name="point">点坐标</param>
	/// <param name="linePoint1">直线上一个点的坐标</param>
	/// <param name="linePoint2">直线上另一个点的坐标</param>
	/// <returns></returns>
	public static float DisPoint2Line(Vector3 point,Vector3 linePoint1,Vector3 linePoint2)
	{
		Vector3 vec1 = point - linePoint1;
		Vector3 vec2 = linePoint2 - linePoint1;
		Vector3 vecProj = Vector3.Project(vec1, vec2);
		float dis =  Mathf.Sqrt(Mathf.Pow(Vector3.Magnitude(vec1), 2) - Mathf.Pow(Vector3.Magnitude(vecProj), 2));
		return dis;
	}
	/// <summary>
	/// 圆和多边形的碰撞
	/// </summary>
	/// <returns><c>true</c>, if circle and polygon was collided, <c>false</c> otherwise.</returns>
	/// <param name="_c1">圆心.</param>
	/// <param name="_radius1">圆半径.</param>
	/// <param name="vectexs">多边形顶点.</param>
	/// <param name="normalDir">多边形每条边的法向量.</param>
	/// <param name="_amend">位置修正.</param>
	public static bool CollideCircleAndPolygon(GameVector2 _c1,int _radius1,GameVec2Con[] vectexs,int[] normalDir ,  out GameVector2 _amend){
		_amend = GameVector2.zero;
		Vector3 _circleCenter = ToolGameVector.ChangeGameVectorToVector3 (_c1);
		for (int i = 0; i < vectexs.Length; i++) {
			Vector3 lineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [i]);
			int endIndex = (i + 1) % vectexs.Length;
			Vector3 lineEnd = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [endIndex]);

			Vector3 _circleVec = _circleCenter - lineBegin;
			Vector3 _line = lineEnd - lineBegin;
			Vector3 _cross = Vector3.Cross (_circleVec, _line);
			if (_cross.z < 0) {
				//在线段的左侧,即多边形的外侧
				Vector3 vecProj = Vector3.Project (_circleVec, _line);//投影点
				float disLine = _line.magnitude;
				float proj_begin = vecProj.magnitude;
				float proj_end = (vecProj - _line).magnitude;
				float projlengh = proj_begin + proj_end;
				if ((disLine + 1) >= projlengh) {
					//投影在线段上
					int dis = (int)Mathf.Sqrt (_circleVec.sqrMagnitude - vecProj.sqrMagnitude);
					int disRadius = _radius1 / 100;
					if (dis < disRadius) {
						int amendDis = 1 + (int)(_radius1 / 100 - dis);
						_amend = amendDis * BattleData.Instance.GetSpeed (normalDir [i]);
						return true;
					} else {
						return false;
					}
				} else {
					Vector3 newLineBegin;
					Vector3 newLineEnd;
					//投影不在当前线段上
					bool isSameDir = Vector3.Dot (vecProj, _line) > 0f ? true : false;//为0的时候是垂直,不会出现该情况
					int normalIndex;
					int linePointIndex;//2条线段的交点
					if (isSameDir) {
						//同向
						newLineBegin = lineEnd;
						int newEndIndex = (i + 2) % vectexs.Length;
						newLineEnd = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [newEndIndex]);
						normalIndex = endIndex;
						linePointIndex = endIndex;
					} else {
						//反向
						if (i == 0) {
							newLineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [vectexs.Length - 1]);	
						} else {
							newLineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [i - 1]);	
						}
						newLineEnd = lineBegin;
						normalIndex = vectexs.Length - 1;
						linePointIndex = i;
					} 
					Vector3 newCircleVec = _circleCenter - newLineBegin;
					Vector3 _newline = newLineEnd - newLineBegin;
					Vector3 newVecProj = Vector3.Project (newCircleVec, _newline);//投影点
					float newdisLine = _newline.magnitude;
					float newproj_begin = newVecProj.magnitude;
					float newproj_end = (newVecProj - _newline).magnitude;
					float newprojlengh = newproj_begin + newproj_end;
					if ((newdisLine + 1) >= newprojlengh) {
						//投影在线段上
						int dis = (int)Mathf.Sqrt (newCircleVec.sqrMagnitude - newVecProj.sqrMagnitude);
						int disRadius = _radius1 / 100;
						if (dis < disRadius) {
							int amendDis = 1 + (int)(_radius1 / 100 - dis);
							_amend = amendDis * BattleData.Instance.GetSpeed (normalDir [normalIndex]);
							return true;
						} else {
							return false;
						}
					} else {
						bool isNewSameDir = Vector3.Dot (newVecProj, _newline) > 0f ? true : false;
						if (isNewSameDir != isSameDir) {
							//夹角处
							GameVector2 _point = ToolGameVector.ChangeGameVectorConToGameVector2 (vectexs [linePointIndex]);
							GameVector2 _outAmend;
							bool _result = ToolGameVector.CollidePointAndCircle (_c1, _point, _radius1, out _outAmend);
							_amend = _outAmend;
							return _result;	
						}
					}
				}
			}
		}
		return false;
	}

	//圆和多边形碰撞
	public static bool CollideCircleAndPolygon(GameVector2 _c1,int _radius1,GameVec2Con[] vectexs,int[] normalDir){
		Vector3 _circleCenter = ToolGameVector.ChangeGameVectorToVector3 (_c1);
		bool isAllIn = true;//是否都在多边形线段的右侧
		for (int i = 0; i < vectexs.Length; i++) {
			Vector3 lineBegin = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [i]);
			int endIndex = (i + 1) % vectexs.Length;
			Vector3 lineEnd = ToolGameVector.ChangeGameVectorConToVector3 (vectexs [endIndex]);

			Vector3 _circleVec = _circleCenter - lineBegin;
			Vector3 _line = lineEnd - lineBegin;
			Vector3 _cross = Vector3.Cross (_circleVec, _line);
			if (_cross.z < 0) {
				isAllIn = false;
				//在线段的左侧,即多边形的外侧
				Vector3 vecProj = Vector3.Project (_circleVec, _line);//投影点
				float disLine = _line.magnitude;
				float proj_begin = vecProj.magnitude;
				float proj_end = (vecProj - _line).magnitude;
				float projlengh = proj_begin + proj_end;
				if ((disLine + 1) >= projlengh) {
					//投影在线段上
					int dis = (int)Mathf.Sqrt (_circleVec.sqrMagnitude - vecProj.sqrMagnitude);
					int disRadius = _radius1 / 100;
					if (dis < disRadius) {
						return true;
					} else {
						return false;
					}
				} else {
					//投影不在当前线段上
					if (CollidePointAndCircle (ChangeGameVectorConToGameVector2 (vectexs [i]), _c1, _radius1)) {
						return true;
					} else {
						return CollidePointAndCircle(ChangeGameVectorConToGameVector2(vectexs [endIndex]),_c1,_radius1);
					}
				}
			}
		}

		if (isAllIn) {
			//全在内侧,圆心在多边形内
			return true;
		}
		return false;
	}

	/// <summary>
	/// 圆和正圆弧的碰撞,带位置修正
	/// </summary>
	/// <returns><c>true</c>, if circle and arc was collided, <c>false</c> otherwise.</returns>
	/// <param name="_c1">圆心.</param>
	/// <param name="_radius1">圆半径.</param>
	/// <param name="_arcCen">圆弧圆心.</param>
	/// <param name="_arcRadius">圆弧所在圆半径.</param>
	/// <param name="_arcAngle">角度.</param>
	/// <param name="_arcAngleSize">角度范围.</param>
	/// <param name="_amend">修正值.</param>
	public static bool CollideCircleAndArc(GameVector2 _c1,int _radius1,GameVector2 _arcCen,int _arcRadius,int _arcAngle,int _arcAngleSize,out GameVector2 _amend){
		_amend = GameVector2.zero;
		int radiusSum = _radius1 + _arcRadius;
		long sqrRadiusSum = radiusSum * radiusSum;
		long sqrDistance = _c1.sqrMagnitude (_arcCen);
		if (sqrDistance >= sqrRadiusSum) {
			return false;
		}
		int radiusDel = _radius1 - _arcRadius;
		long sqrRadiusDel = radiusDel * radiusDel;
		if (sqrDistance <= sqrRadiusDel) {
			return false;
		}
		int _angle = (_c1 - _arcCen).Angle ();
		int _point1Angle = _arcAngle + _arcAngleSize;
		int _point2Angle = _arcAngle - _arcAngleSize;
		bool inArc;
		if (_point2Angle < 0) {
			int _angle1 = (int)Mathf.Repeat (_angle - _point2Angle,360f);
			if (_angle1 >= 0 && _angle1 <= (_point1Angle - _point2Angle)) {
				inArc = true;
			} else {
				inArc = false;
			}
		} 
		else if(_point1Angle > 360){
			int _delA = _point1Angle - 360;
			int _angle1 = (int)Mathf.Repeat (_angle - _delA,360f);
			if (_angle1 >= (_point2Angle - _delA) && _angle1 <= 360) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		else {
			if (_angle >= _point2Angle && _angle <= _point1Angle) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		if (inArc) {
			//在圆弧内,需要修正
			long _arcRadiusSqr = _arcRadius * _arcRadius;
			int distance = (int)Mathf.Sqrt (sqrDistance);
			int amendDis;
			int amendAngle;
			if (sqrDistance >= _arcRadiusSqr) {
				//向外修正
				amendDis = 1 + (int)(radiusSum - distance) / 100;
				amendAngle = _angle;
			} else {
				amendDis = 1 + (int)(distance - Mathf.Abs(radiusDel)) / 100;
				amendAngle = (_arcCen - _c1).Angle ();
			}
			_amend = amendDis * BattleData.Instance.GetSpeed (amendAngle);
			return true;
		}

		_point1Angle = (int)Mathf.Repeat (_point1Angle, 360);
		GameVector2 _point1 = _arcCen + (_arcRadius/100) * BattleData.Instance.GetSpeed (_point1Angle);

		GameVector2 _outAmend1;
		bool _result1 = ToolGameVector.CollidePointAndCircle (_c1, _point1, _radius1, out _outAmend1);
		if (_result1) {
			_amend = _outAmend1;
			return _result1;	
		}
			
		_point2Angle = (int)Mathf.Repeat (_point2Angle, 360);
		GameVector2 _point2 = _arcCen + (_arcRadius/100) * BattleData.Instance.GetSpeed (_point2Angle);

		GameVector2 _outAmend2;
		bool _result2 = ToolGameVector.CollidePointAndCircle (_c1, _point2, _radius1, out _outAmend2);
		if (_result2) {
			_amend = _outAmend2;
			return _result2;	
		}
		return false;
	}
	// 圆和正圆弧的碰撞,不带位置修正
	public static bool CollideCircleAndArc(GameVector2 _c1,int _radius1,GameVector2 _arcCen,int _arcRadius,int _arcAngle,int _arcAngleSize){
		int radiusSum = _radius1 + _arcRadius;
		long sqrRadiusSum = radiusSum * radiusSum;
		long sqrDistance = _c1.sqrMagnitude (_arcCen);
		if (sqrDistance >= sqrRadiusSum) {
			return false;
		}
		int radiusDel = _radius1 - _arcRadius;
		long sqrRadiusDel = radiusDel * radiusDel;
		if (sqrDistance <= sqrRadiusDel) {
			return false;
		}
		int _angle = (_c1 - _arcCen).Angle ();
		if (Mathf.Abs(_angle - _arcAngle) <= _arcAngleSize) {
			return true;
		}
		int _point1Angle = (int)Mathf.Repeat (_arcAngle + _arcAngleSize, 360);
		GameVector2 _point1 = _arcCen + (_arcRadius/100) *BattleData.Instance.GetSpeed (_point1Angle);
		bool _result1 = ToolGameVector.CollidePointAndCircle (_c1, _point1, _radius1);
		if (_result1) {
			return _result1;	
		}
		int _point2Angle = (int)Mathf.Repeat (_arcAngle - _arcAngleSize, 360);
		GameVector2 _point2 = _arcCen + (_arcRadius/100) * BattleData.Instance.GetSpeed (_point2Angle);
		bool _result2 = ToolGameVector.CollidePointAndCircle (_c1, _point2, _radius1);
		if (_result2) {
			return _result2;	
		}
		return false;
	}

	/// <summary>
	/// 圆和正圆扇面的碰撞,带位置修正
	/// </summary>
	/// <returns><c>true</c>, if circle and arc was collided, <c>false</c> otherwise.</returns>
	/// <param name="_c1">圆心.</param>
	/// <param name="_radius1">圆半径.</param>
	/// <param name="_arcCen">扇面圆心.</param>
	/// <param name="_arcRadius">扇面所在圆半径.</param>
	/// <param name="_arcAngle">角度.</param>
	/// <param name="_arcAngleSize">角度范围.</param>
	/// <param name="_amend">修正值.</param>
	public static bool CollideCircleAndArcArea(GameVector2 _c1,int _radius1,GameVector2 _arcCen,int _arcRadius,int _arcAngle,int _arcAngleSize,out GameVector2 _amend){
		_amend = GameVector2.zero;
		int radiusSum = _radius1 + _arcRadius;
		long sqrRadiusSum = radiusSum * radiusSum;
		long sqrDistance = _c1.sqrMagnitude (_arcCen);  
		if (sqrDistance >= sqrRadiusSum) {
			return false;
		}
		int _angle = (_c1 - _arcCen).Angle ();
		int _point1Angle = _arcAngle + _arcAngleSize;
		int _point2Angle = _arcAngle - _arcAngleSize;
		bool inArc;
		if (_point2Angle < 0) {
			int _angle1 = (int)Mathf.Repeat (_angle - _point2Angle,360f);
			if (_angle1 >= 0 && _angle1 <= (_point1Angle - _point2Angle)) {
				inArc = true;
			} else {
				inArc = false;
			}
		} 
		else if(_point1Angle > 360){
			int _delA = _point1Angle - 360;
			int _angle1 = (int)Mathf.Repeat (_angle - _delA,360f);
			if (_angle1 >= (_point2Angle - _delA) && _angle1 <= 360) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		else {
			if (_angle >= _point2Angle && _angle <= _point1Angle) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		if (inArc) {
			//在圆弧内,需要修正
			int distance = (int)Mathf.Sqrt (sqrDistance);
			int amendDis = 1 + (int)(radiusSum - distance) / 100;
			_amend = amendDis * BattleData.Instance.GetSpeed (_angle);
			return true;
		}
		//线段修正
		Vector3 _circleCenter = ToolGameVector.ChangeGameVectorToVector3 (_c1);
		Vector3 _arcAreaCenter = ToolGameVector.ChangeGameVectorToVector3 (_arcCen);

		_point1Angle = (int)Mathf.Repeat (_point1Angle, 360);
		GameVector2 _point1 = _arcCen + (_arcRadius/100) * BattleData.Instance.GetSpeed (_point1Angle);

		_point2Angle = (int)Mathf.Repeat (_point2Angle, 360);
		GameVector2 _point2 = _arcCen + (_arcRadius/100) * BattleData.Instance.GetSpeed (_point2Angle);

		Vector3 _line1Point = ToolGameVector.ChangeGameVectorToVector3 (_point1);
		Vector3 _line2Point = ToolGameVector.ChangeGameVectorToVector3 (_point2);
		//线段1修正
		bool amendline1 = false;
		GameVector2 amendLine1P = GameVector2.zero;
		Vector3 _circleVec = _circleCenter - _arcAreaCenter;
		Vector3 _line = _line1Point - _arcAreaCenter;
		Vector3 _cross = Vector3.Cross (_circleVec, _line);
		if (_cross.z <= 0) {
			//在线段的左侧,即多边形的外侧
			Vector3 vecProj = Vector3.Project (_circleVec, _line);//投影点
			float disLine = _line.magnitude;
			float proj_begin = vecProj.magnitude;
			float proj_end = (vecProj - _line).magnitude;
			float projlengh = proj_begin + proj_end;
			if ((disLine + 1) >= projlengh) {
				//投影在线段上
				int dis = (int)Mathf.Sqrt (_circleVec.sqrMagnitude - vecProj.sqrMagnitude);
				int disRadius = _radius1 / 100;
				if (dis < disRadius) {
					//需要修正
					amendline1 = true;
					int amendDis = 1 + (int)(_radius1 / 100 - dis);
					Vector3 angelVec = _circleVec - vecProj;
					float angle1 = Mathf.Atan2 (angelVec.y, angelVec.x) * Mathf.Rad2Deg;
					int amendAngle = (int)Mathf.Repeat(angle1,360f);
					amendLine1P = amendDis * BattleData.Instance.GetSpeed (amendAngle);
				}
			} else {
				//投影不在当前线段上
			}
		}
		//线段2修正
		bool amendline2 = false;
		GameVector2 amendLine2P = GameVector2.zero;
		Vector3 _circleVec2 = _circleCenter - _line2Point;
		Vector3 _line2 = _arcAreaCenter - _line2Point;
		Vector3 _cross2 = Vector3.Cross (_circleVec2, _line2);
		if (_cross2.z <= 0) {
			//在线段的左侧,即多边形的外侧
			Vector3 vecProj = Vector3.Project (_circleVec2, _line2);//投影点
			float disLine = _line2.magnitude;
			float proj_begin = vecProj.magnitude;
			float proj_end = (vecProj - _line2).magnitude;
			float projlengh = proj_begin + proj_end;
			if ((disLine + 1) >= projlengh) {
				//投影在线段上

				int dis = (int)Mathf.Sqrt (_circleVec2.sqrMagnitude - vecProj.sqrMagnitude);
				int disRadius = _radius1 / 100;
				if (dis < disRadius) {
					//需要修正
					amendline2 = true;
					int amendDis = 1 + (int)(_radius1 / 100 - dis);
					Vector3 angelVec = _circleVec2 - vecProj;
					float angle1 = Mathf.Atan2 (angelVec.y, angelVec.x) * Mathf.Rad2Deg;
					int amendAngle = (int)Mathf.Repeat(angle1,360f);
					amendLine2P = amendDis * BattleData.Instance.GetSpeed (amendAngle);
				}
			} else {
				//投影不在当前线段上
			}
		}
		if (amendline1||amendline2) {
			_amend = amendLine1P + amendLine2P;
			return true;
		}
		//对点修正
		GameVector2 _outAmend1;
		bool _result1 = ToolGameVector.CollidePointAndCircle (_c1, _arcCen, _radius1,out _outAmend1);
		if (_result1) {
			_amend = _outAmend1;
			return true;	
		}
		GameVector2 _outAmend2;
		bool _result2 = ToolGameVector.CollidePointAndCircle (_c1, _point1, _radius1,out _outAmend2);

		GameVector2 _outAmend3;
		bool _result3 = ToolGameVector.CollidePointAndCircle (_c1, _point2, _radius1,out _outAmend3);

		if (_result2 || _result3) {
			_amend = _outAmend2 + _outAmend3;
			return true;
		}
		return false;
	}

	// 圆和正圆扇面的碰撞,不带位置修正
	public static bool CollideCircleAndArcArea(GameVector2 _c1,int _radius1,GameVector2 _arcCen,int _arcRadius,int _arcAngle,int _arcAngleSize){
		int radiusSum = _radius1 + _arcRadius;
		long sqrRadiusSum = radiusSum * radiusSum;
		long sqrDistance = _c1.sqrMagnitude (_arcCen);

		if (sqrDistance >= sqrRadiusSum) {
			return false;
		}
		int _angle = (_c1 - _arcCen).Angle ();
		int _point1Angle = _arcAngle + _arcAngleSize;
		int _point2Angle = _arcAngle - _arcAngleSize;

		bool inArc;
		if (_point2Angle < 0) {
			int _angle1 = (int)Mathf.Repeat (_angle - _point2Angle,360f);
			if (_angle1 >= 0 && _angle1 <= (_point1Angle - _point2Angle)) {
				inArc = true;
			} else {
				inArc = false;
			}
		} 
		else if(_point1Angle > 360){
			int _delA = _point1Angle - 360;
			int _angle1 = (int)Mathf.Repeat (_angle - _delA,360f);
			if (_angle1 >= (_point2Angle - _delA) && _angle1 <= 360) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		else {
			if (_angle >= _point2Angle && _angle <= _point1Angle) {
				inArc = true;
			} else {
				inArc = false;
			}
		}
		if (inArc) {
			return true;
		}
		return false;
	}
}
