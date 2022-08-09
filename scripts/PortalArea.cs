using Godot;
using System;

public class PortalArea : Area
{
	private AudioStreamPlayer AudioSuck;
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
		AudioSuck = GetNode<AudioStreamPlayer>("AudioSuck");
	}
	
	private void on_PortalArea(object body)
	{
		AudioSuck.Play();
		gb.EmitSignal("EnteredPortal");
	}
}
