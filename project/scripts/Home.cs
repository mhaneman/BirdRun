using Godot;
using System.Collections.Generic;

public class Home : Spatial
{
	[Signal]public delegate void PlayerInput(string direction);
	[Signal]public delegate void SummonEgg();
	
	private Node SwipeDetector;
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
		
		SwipeDetector = GetNode<Node>("SwipeDetector");
		SwipeDetector.Connect("Swiped", this, "on_Swiped");
	}
	
	private void on_Swiped(string direction)
	{
		if (direction != "jump")
			return;
			
		EmitSignal("PlayerInput", direction);
		EmitSignal("SummonEgg");
	}
	
	private void on_HomePortal(object body)
	{
		GetTree().ChangeScene("res://scenes/Game.tscn");
	}
}
