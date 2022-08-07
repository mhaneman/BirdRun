using Godot;
using System;

public class PortalArea : Area
{
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
	}
	
	private void on_PortalArea(object body)
	{
		gb.EmitSignal("EnteredPortal");
	}
}
