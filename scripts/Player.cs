using Godot;
using System;

public class Player : KinematicBody
{
	
	float gravity = 1.3f;
	float jump_power = 35f;
	float max_terminal_velocity = 60f;
	float egg_power = 10f;
	
	private float y_vel;
	private Vector3 velocity;
	private float theta = 0f;
	
	AudioStreamPlayer Switch;
	AudioStreamPlayer Jump;
	AudioStreamPlayer Death;
	
	private PackedScene eggScene;
	private Spatial eggSpawnPoint;
	
	private GameControl gc;
	private GameBus gb;
	public override void _Ready()
	{
		gc = GetNode<GameControl>("/root/GameControl");
		gb = GetNode<GameBus>("/root/GameBus");
		gb.Connect("EnteredPortal", this, "on_EnteredPortal");
		
		Death = GetNode<AudioStreamPlayer>("Death");
		Jump = GetNode<AudioStreamPlayer>("Jump");
		Switch = GetNode<AudioStreamPlayer>("Switch");
		
		eggScene = ResourceLoader.Load<PackedScene>("res://scenes/Player/Egg.tscn");
		eggSpawnPoint = GetNode<Spatial>("EggSpawnPoint");
	}
	
	public override void _Process(float delta)
	{
		Lerp();
	}
	
	private void Lerp()
	{
		Vector3 rot = this.Rotation;
		rot.y = Mathf.LerpAngle(rot.y, theta, gc.speed * (1f / gc.lerp_weight));
		this.Rotation = rot;
	}
	
	private void on_PlayerInput(string direction)
	{
		if (direction == "left")
		{
			theta += Mathf.Pi/2;
			Switch.Play();	
		}
		if (direction == "right")
		{
			theta -= Mathf.Pi/2;	
			Switch.Play();
		}
		
		if (direction == "jump")
		{
			y_vel = jump_power;
			Jump.Play();
		}
		
		if (direction == "down")
			y_vel = -jump_power;	
	}
	
	private void on_EnteredPortal()
	{
		theta = 0;
		Transform t = this.GlobalTransform;
		t.origin = new Vector3(0, 10, 0);
		this.GlobalTransform = t;
	}
	
	private void on_SummonEgg()
	{
		RigidBody egg = eggScene.Instance<RigidBody>();
		GetTree().Root.AddChild(egg);
		egg.GlobalTransform = eggSpawnPoint.GlobalTransform;
		egg.ApplyCentralImpulse(eggSpawnPoint.GlobalTransform.basis.y * egg_power);
	}
	
	
	public override void _PhysicsProcess(float delta)
	{
		handle_gravity();
	}
	
	private void handle_gravity()
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
