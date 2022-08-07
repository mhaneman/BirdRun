using Godot;
using System;

public class GameControl : Node
{
	public bool IsOnFloor = false;
	public float speed = 20f;
	public float lerp_weight = 230f; // greater the value, the slower the player rotation
}
