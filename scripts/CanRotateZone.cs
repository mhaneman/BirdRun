using Godot;
using System;

public class CanRotateZone : Area
{
	private GameControl gc;
	public override void _Ready()
	{
		gc = GetNode<GameControl>("/root/GameControl");
	}
	
	private void _on_CanRotateZone_body_entered(object body)
	{
		gc.InRotationZone = true;
	}


	private void _on_CanRotateZone_body_exited(object body)
	{
		gc.InRotationZone = false;
	}
}
