using Godot;
using System;

public class Camera : Godot.Camera
{
	public override void _Ready()
	{
		
	}
	
	public override void _Process(float delta)
	{
		if (Input.IsActionPressed("left"))
		{
			Transform t = this.GlobalTransform;
			t.origin.x -= 40f * delta;
			this.GlobalTransform = t;
		}
		
		if (Input.IsActionPressed("right"))
		{
			Transform t = this.GlobalTransform;
			t.origin.x += 40f * delta;
			this.GlobalTransform = t;
		}
		
		if (Input.IsActionPressed("up"))
		{
			Transform t = this.GlobalTransform;
			t.origin.z -= 40f * delta;
			this.GlobalTransform = t;
		}
		
		if (Input.IsActionPressed("down"))
		{
			Transform t = this.GlobalTransform;
			t.origin.z += 40f * delta;
			this.GlobalTransform = t;	
		}
		
		if (Input.IsActionPressed("space"))
		{
			Transform t = this.GlobalTransform;
			t.origin.y += 40f * delta;
			this.GlobalTransform = t;	
		}
		
		if (Input.IsActionPressed("shift"))
		{
			Transform t = this.GlobalTransform;
			t.origin.y -= 40f * delta;
			this.GlobalTransform = t;	
		}
	}
}
