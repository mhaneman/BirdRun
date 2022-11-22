using Godot;
using System;

public class ChunkSpace : Spatial
{
	private float speed;
	private Vector2 Direction;
	private float Inc = -Mathf.Pi/2;
	private float Trans = 4f;
	private float Theta = 0f;
	
	private GameControl gc;
	private GameBus gb;
	public override void _Ready()
	{
		gc = GetNode<GameControl>("/root/GameControl");
		gb = GetNode<GameBus>("/root/GameBus");
		gb.Connect("EnteredPortal", this, "on_EnteredPortal");
		
		this.speed = gc.speed;
		Direction = new Vector2(0f, speed);
	}
	
	private void on_PlayerInput(string direction)
	{
		if (direction == "left")
			Theta -= Inc;
			
		if (direction == "right")
			Theta += Inc;
	}
	
	private void on_EnteredPortal()
	{
		Theta = 0f;
		Transform t = this.GlobalTransform;
		t.origin = new Vector3(0, 0, 0);
		this.GlobalTransform = t;
	}
	
	public override void _PhysicsProcess(float delta)
	{
		Transform t = this.Transform;
		
		/*
		if (!gc.InRotationZone)
		{
			if (Input.IsActionJustPressed("left"))
			{
				t.origin.z += Trans * Mathf.Sin(Theta);
				t.origin.x += Trans * Mathf.Cos(Theta);
			}

			if (Input.IsActionJustPressed("right"))
			{
				t.origin.z -= Trans * Mathf.Sin(Theta);
				t.origin.x -= Trans * Mathf.Cos(Theta);
			}
		} */
		
		// move the world at constant velocity
		t.origin.z += speed * delta * Mathf.Cos(Theta);
		t.origin.x += speed * delta * Mathf.Sin(Theta);
		this.Transform = t;
	}
}
