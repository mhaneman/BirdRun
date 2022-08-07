using Godot;
using System.Collections.Generic;
using System;

public class PathBuilder : Spatial
{
	Random rand = new Random();
	private PathSpawner<StaticBody> pathSpawner;
	private LinkedList<Vector3> pathFollow;

	private float[] directions = {-Mathf.Pi/2, 0f, Mathf.Pi/2};
	private int numOfPlatforms;
	
	private Vector3 end = new Vector3(0, 100, 0);
	private float withinReachArea = 20f;
	private float platformSpacing = 14f;

	private float saltWeight = 0.8f;

	private int numProcess = 0;
	private bool runProcess = true;
	private bool endProcess = false;
	
	private GameBus gb;
	public override void _Ready()
	{
		gb = GetNode<GameBus>("/root/GameBus");
		gb.Connect("EnteredPortal", this, "on_EnteredPortal");
		
		pathSpawner = new PathSpawner<StaticBody>(this);
		numOfPlatforms = pathSpawner.PlatformTypes.Count;
	}
	
	private void on_EnteredPortal()
	{
		pathSpawner.Reset();
		endProcess = false;
		numProcess = 0;
		runProcess = true;
	}
	
	public override void _Process(float delta)
	{
		if (!runProcess)
			return;

		initalPlatforms(pathSpawner);
		
		addPlatform();

		if (endProcess || numProcess >= 30)
		{
			pathSpawner.SummonPortal();
			runProcess = false;
		} 
		numProcess++;
	}

	private void initalPlatforms(PathSpawner<StaticBody> pathSpawner)
	{
		if (numProcess == 0)
		{
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
		}
	}

	private void addPlatform()
	{
		var platforms = new PriorityQueue<(int, float)>(true);
		foreach(var potentialPlatform in pathSpawner.getAllPlatformConnectingGlobalPoints())
		{
			var plat = potentialPlatform.Value;
			
			if (!IsOverlapping(plat))
			{
				setIsWithinReach(plat);
				platforms.Enqueue(ratePlatform(plat), potentialPlatform.Key);
			}
		}
		
		// if all platforms collide with enviroment -> summon portal
		if (platforms.Count == 0)
		{
			endProcess = true;
			pathSpawner.SummonPortal();
		}
		
		(int platformType, float rotation) newPlatform = platforms.Dequeue();
		pathSpawner.Summon(newPlatform.platformType, newPlatform.rotation);
	}

	private int ratePlatform(Vector3 currentPos)
	{
		float distance = currentPos.DistanceTo(end);
		float salt = saltWeight * distance * (float) rand.NextDouble();
		return (int) (distance + salt);
	}

	private void setIsWithinReach(Vector3 position)
	{
		if (position.DistanceTo(end) <= withinReachArea)
			endProcess = true;
	}
	
	private bool IsOverlapping(Vector3 position)
	{
		foreach (StaticBody s in pathSpawner.ActivePlatforms)
		{
			if (position.DistanceTo(s.GlobalTransform.origin) <= platformSpacing)
				return true;
		}
		return false;
	}
}
