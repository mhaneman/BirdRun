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
		gb.Connect("EnteredPortal", this, "on_EnteredPortal");
		gb.Connect("PlayerDied", this, "on_PlayerDied");
		
		gc = GetNode<GameControl>("/root/GameControl");
	}
	
	private void on_Swiped(string direction)
	{
		if (direction != "jump" || gb.IsOnFloor)
		{
			EmitSignal("PlayerInput", direction);
			return;
		}
		
		if (gc.eggs > 0)
		{
			gc.eggs--;
			EmitSignal("PlayerInput", direction);
			EmitSignal("SummonEgg");
		}
	}
	
	private void on_PlayerDied()
	{
		GD.Print("died");
		GetTree().ChangeScene("res://scenes/Menus/Home.tscn");
	}
	
	private void on_EnteredPortal()
	{
		GD.Print("game portal");
	}
}
