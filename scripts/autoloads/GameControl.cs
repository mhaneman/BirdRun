using Godot;
using System;

// this class is for tweaking the game
public class GameControl : Node
{
	public float speed = 20f;
	public float lerp_weight = 230f; // greater the value, the slower the player rotation

	
	public int eggs = 20;
	public int fruits = 0;
}
