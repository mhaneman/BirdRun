using Godot;
using System;

public class DeathFloor : Area
{
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
	}
	
	private void _on_DeathFloor_body_entered(object body)
	{
		gb.EmitSignal("PlayerDied");
		GD.Print("died");
	}
	
}
