using Godot;
using System;

public class Egg : RigidBody
{	
	private void _on_Egg_body_entered(object body)
	{
		this.QueueFree(); // destroy bullet
	}
}
