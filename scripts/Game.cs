using Godot;
using System;

public class Game : Spatial
{
	[Signal]public delegate void PlayerInput(string direction);
	[Signal]public delegate void SummonEgg();
	
	private GameBus gb;
	private GameControl gc;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
		gc = GetNode<GameControl>("/root/GameControl");
	}
	
	private void on_Swiped(string direction)
	{
		if (direction != "jump" || gc.IsOnFloor)
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
	
	private void on_BodyEntered(object body)
	{
		GD.Print("died");
		GetTree().ChangeScene("res://scenes/Menus/Home.tscn");
	}
}
