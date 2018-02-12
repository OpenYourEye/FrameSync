using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolMethod {
	//配置的数值单位1像素，logic单位0.01像素，unity单位100像素
	public static readonly int Config2LogicScale = 100;		
	public static readonly float Logic2ConfigScale = 0.01f;	

	public static readonly int Render2ConfigScale = 100;		
	public static readonly float Config2RenderScale = 0.01f;

	public static readonly int Render2LogicScale = 10000;
	public static readonly float Logic2RenderScale = 0.0001f;

	public static int Config2Logic(int _value){
		return _value * Config2LogicScale;
	}

	public static int Logic2Config(int _value){
		return (int)(_value * Logic2ConfigScale);
	}

	public static int Render2Config(float _value){
		return (int)(_value * Render2ConfigScale);
	}

	public static float Config2Render(int _value){
		return _value * Config2RenderScale;
	}


	public static int Render2Logic(float _value){
		return (int)(_value * Render2LogicScale);
	}

	public static float Logic2Render(int _value){
		return _value * Logic2RenderScale;
	}



	public static int FollowAngle(int _objAngle,int _tarAngle,int _speed)
	{
		int delAngle = _tarAngle - _objAngle;

		if (Mathf.Abs(delAngle) <= _speed) {
			return _tarAngle;
		}
		int angleSign = System.Math.Sign(delAngle);

		if (delAngle > 180 || delAngle < -180) {
			angleSign *= -1;
		}

		int angle = _objAngle + angleSign * _speed;
		return (int)Mathf.Repeat(angle,360f);
	}


	public static float FollowAngle(float _objAngle,float _tarAngle,float _speed)
	{
		float delAngleZ = _objAngle - _tarAngle;
		float _fixangle = _objAngle;
		if (delAngleZ > 180) {
			_fixangle = _objAngle - 360f;
		} else if (delAngleZ < -180){
			_fixangle = _objAngle + 360f;
		}

		return Mathf.Lerp (_fixangle,_tarAngle,_speed);
	}
}
