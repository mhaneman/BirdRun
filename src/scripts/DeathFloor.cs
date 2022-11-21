using Godot;
using System;

public class DeathFloor : Area
{
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
	}
	
	private void on_DeathFloor(object body)
	{
		gb.EmitSignal("PlayerDied");
	}
}

