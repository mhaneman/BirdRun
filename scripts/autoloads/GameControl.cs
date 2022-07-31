using Godot;
using System;

public class GameControl : Node
{
	public bool InRotationZone = false;
	public bool IsOnFloor = false;
	public float speed = 20f;
	public float lerp_weight = 0.007f;
}
