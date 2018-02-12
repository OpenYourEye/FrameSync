using UnityEngine;
using System.Collections;

[System.Serializable]
public struct GameVec2Con{
	public int x;
	public int y;
}

[System.Serializable]
public struct GameVector2{
	//	private string name;

	public int x;
	public int y;


	public GameVector2 (int _x, int _y){
		x = _x;
		y = _y;
	}

	public GameVector2 (float _x, float _y){
		x = (int)_x;
		y = (int)_y;
	}

	public GameVector2 (Vector2 _vec2){
		x = (int)_vec2.x;
		y = (int)_vec2.y;
	}

	public GameVector2 (Vector3 _vec3){
		x = (int)_vec3.x;
		y = (int)_vec3.y;
	}

	public static GameVector2 zero {
		get
		{ 
			return new GameVector2 (0,0);
		}
	}

	public static GameVector2 one {
		get
		{ 
			return new GameVector2 (1,1);
		}
	}

	public override bool Equals (object other){
		if (other is GameVector2)
		{
			return Equals((GameVector2)other);
		}
		return false;
	}

	public bool Equals(GameVector2 obj)
	{
		return this.x == obj.x && this.y == obj.y;
	}

	/// <summary>
	/// 长度
	/// </summary>
	public float magnitude(GameVector2 obj){
		return Mathf.Sqrt ((this.x - obj.x) * (this.x - obj.x) + (this.y - obj.y) * (this.y - obj.y));
	}

	/// <summary>
	/// 长度平方
	/// </summary>
	public long sqrMagnitude(GameVector2 obj){
		long dx = this.x - obj.x;
		long dy = this.y - obj.y;
		return dx * dx + dy * dy;
	}

	public long sqrMagnitude(){
		long dx = this.x;
		long dy = this.y;
		return dx * dx + dy * dy;
	}
	/// <summary>
	/// 返回0~359的角度值
	/// </summary>
	public int Angle(){
		float angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;
		return (int)Mathf.Repeat(angle,360f);
	}

	public int Angle(GameVector2 _target){
		return (_target - this).Angle ();
	}
		
	public override int GetHashCode() 
	{
		return this.ToString().GetHashCode();
	}


	public override string ToString (){
		return x.ToString () + "," + y.ToString ();
	}

	//
	// Operators
	//
	public static GameVector2 operator + (GameVector2 a, GameVector2 b){
		return new GameVector2 (a.x + b.x , a.y + b.y);
	}

	//	public static GameVector2 operator / (GameVector2 a, float d);

	public static bool operator == (GameVector2 lhs, GameVector2 rhs){
		return lhs.x == rhs.x && lhs.y == rhs.y;
	}

	public static bool operator != (GameVector2 lhs, GameVector2 rhs){
		return lhs.x != rhs.x || lhs.y != rhs.y;
	}

	public static GameVector2 operator * (int d, GameVector2 a){
		return new GameVector2 (a.x * d , a.y * d);
	}

	public static GameVector2 operator * (GameVector2 a, int d){
		return new GameVector2 (a.x * d , a.y * d);
	}

	public static GameVector2 operator - (GameVector2 a, GameVector2 b){
		return new GameVector2 (a.x - b.x , a.y - b.y);
	}
}
