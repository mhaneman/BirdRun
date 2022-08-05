using Godot;
using System.Collections.Generic;
using System;

public class PathBuilder : Spatial
{
	Random rand = new Random();
	private PathSpawner<StaticBody> pathSpawner;
	private LinkedList<Vector3> pathFollow;

	float[] directions = {-Mathf.Pi/2, 0f, Mathf.Pi/2};
	int numOfPlatforms;
	
	public override void _Ready()
	{
		
		pathSpawner = new PathSpawner<StaticBody>(this);
		numOfPlatforms = pathSpawner.PlatformTypes.Count;
	}

	private Vector3 end = new Vector3(30, 40, 100);
	private float withinReachArea = 20f;
	private bool withinReach = false;

	private float saltWeight = 0.8f;

	private int numProcess = 0;
	private bool runProcess = true;
    public override void _Process(float delta)
    {
		if (!runProcess)
			return;

		initalPlatforms(pathSpawner);
		
		addPlatform();

		if (withinReach || numProcess >= 30)
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
		}
	}

	private void addPlatform()
	{
		var platforms = new PriorityQueue<(int, float)>(true);
		foreach(var potentialPlatform in pathSpawner.getAllPlatformConnectingGlobalPoints())
		{
			var plat = potentialPlatform.Value;
			setIsWithinReach(plat);
			platforms.Enqueue(ratePlatform(plat), potentialPlatform.Key);
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

	private void setIsWithinReach(Vector3 currentPos)
	{
		if (currentPos.DistanceTo(end) <= withinReachArea)
			withinReach = true;
	}
}
