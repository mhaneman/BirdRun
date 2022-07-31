using Godot;
using System;

public class GameControl : Node
{
	[Signal]public delegate void PlayerInput(string direction);
	[Signal]public delegate void SummonEgg();
	
	public bool InRotationZone = false;
	public bool IsOnFloor = false;
	public float speed = 20f;
	public float lerp_weight = 0.007f;
	
	private GameBus gb;
	private Node swipe_detector;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
		
		swipe_detector = GetNode<Node>("/root/World/SwipeDetector");
		swipe_detector.Connect("Swiped", this, "on_Swiped");
	}
	
	private void on_Swiped(string direction)
	{
		if (direction == "down" || IsOnFloor)
		{
			EmitSignal("PlayerInput", direction);
			return;
		}
		
		if (gb.eggs > 0)
		{
			gb.eggs--;
			EmitSignal("PlayerInput", direction);
			EmitSignal("SummonEgg");
		}
	}
	
	
}
