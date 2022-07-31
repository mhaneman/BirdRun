using Godot;
using System;

public class Player : KinematicBody
{
	
	float gravity = 1.3f;
	float jump_power = 35f;
	float max_terminal_velocity = 60f;
	
	private float y_vel;
	private Vector3 velocity;
	
	private float theta = 0f;
	
	private GameControl gc;
	public override void _Ready()
	{
		gc = GetNode<GameControl>("/root/GameControl");
	}
	
	public override void _Process(float delta)
	{
		Controls();
		Lerp();
	}
	
	private void OnSwiped(string direction)
	{
		
		if (direction == "left")
			theta += Mathf.Pi/2;
		if (direction == "right")
			theta -= Mathf.Pi/2;
		if (direction == "jump")
			y_vel = jump_power;
		if (direction == "down")
			y_vel = -jump_power;
	}
	
	private void Controls()
	{
		if (gc.IsOnFloor || gc.InRotationZone)
		{
			if (Input.IsActionJustPressed("left"))
				theta += Mathf.Pi/2;
		
			if (Input.IsActionJustPressed("right"))
				theta -= Mathf.Pi/2;
		}
		
		if (Input.IsActionJustPressed("space") && (IsOnFloor() || gc.InRotationZone)) 
			y_vel = jump_power;
		
		if (Input.IsActionJustPressed("down") && !IsOnFloor())
			y_vel = -jump_power;
		
	}
	
	private void Lerp()
	{
		Vector3 rot = this.Rotation;
		rot.y = Mathf.LerpAngle(rot.y, theta, gc.speed * gc.lerp_weight);
		this.Rotation = rot;
	}
	
	
	public override void _PhysicsProcess(float delta)
	{
		if (IsOnFloor())
		{
			gc.IsOnFloor = true;
			velocity.y = 0f;	
		}
		else
		{
			gc.IsOnFloor = false;
			y_vel = Mathf.Clamp(y_vel-gravity, -max_terminal_velocity, jump_power);
		}
			
		velocity.y = y_vel;
		MoveAndSlide(velocity, Vector3.Up, false, 4, 1.5605f, false);
	}
}
