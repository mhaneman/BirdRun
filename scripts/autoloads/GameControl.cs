using Godot;
using System;

public class GameControl : Node
{
	public bool IsOnFloor = false;
	public float speed = 35f;
	public float lerp_weight = 200f; // greater the value, the slower the player rotation
}
