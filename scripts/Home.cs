using Godot;
using System;

public class Home : Spatial
{
	[Signal]public delegate void PlayerInput(string direction);
	[Signal]public delegate void SummonEgg();
	
	private void on_Swiped(string direction)
	{
		if (direction == "down")
			return;
			
		EmitSignal("PlayerInput", direction);
		EmitSignal("SummonEgg");
	}
	
	private void on_Pressed()
	{
		GetTree().ChangeScene("res://scenes/Game.tscn");
	}
}
