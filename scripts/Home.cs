using Godot;
using System.Collections.Generic;

public class Home : Spatial
{
	[Signal]public delegate void PlayerInput(string direction);
	[Signal]public delegate void SummonEgg();
	
	Pathing path = new Pathing();

	public override void _Ready()
	{
		var p = path.findGoal();
		foreach(Vector3 i in p)
			GD.Print(i);
	}
	
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
