using Godot;
using System.Collections.Generic;
using System;

public class PathBuilder : Spatial
{
	private Pathing pathing = new Pathing();
	private PathSpawner<StaticBody> pathSpawner;
	private LinkedList<Vector3> pathFollow;

	Random rand = new Random();

	private Vector3 start = new Vector3(0, 0, 0);
	private Vector3 end = new Vector3(100, 100, 100);
	private float sampleArea = 10f;
	
	public override void _Ready()
	{
		
		pathSpawner = new PathSpawner<StaticBody>(this);
		//pathFollow = pathing.findGoal(start, end);
	}

	private bool runProcess = true;
	int i = 0;
    public override void _Process(float delta)
    {
		if (!runProcess)
			return;

		if (i == 0) 
		{
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
			pathSpawner.Summon(0, 0f);
		}

		
		var platforms = new PriorityQueue<(int, float)>(true);
		foreach(var potentialPlatform in pathSpawner.getAllPlatformConnectingGlobalPoints())
			platforms.Enqueue(ratePlatform(potentialPlatform.Value), potentialPlatform.Key);
		(int platformType, float rotation) newPlatform = platforms.Dequeue();
		pathSpawner.Summon(newPlatform.platformType, newPlatform.rotation);


		if (i > 50)
			runProcess = false;
		else 
			i++;
    }

	private int ratePlatform(Vector3 currentPos)
	{
		// List<Vector3> samples = getSamples(currentPos);
		// Vector3 direction = getLocalPathDirection(samples);
		return rand.Next(12);
	}

	// private List<Vector3> getSamples(Vector3 currentPos)
	// {
	// 	List<Vector3> samples = new List<Vector3>();
	// 	foreach(Vector3 i in pathFollow)
	// 	{
	// 		if (i.DistanceTo(currentPos) <= sampleArea)
	// 			samples.Add(i);
	// 	}
	// 	return samples;
	// }

	// private float getAverageDistanceFromSamples(Vector3 currentPos, List<Vector3> samples)
	// {
	// 	float averageDistance = 0f;
	// 	foreach(Vector3 i in samples)
	// 		averageDistance += currentPos.DistanceTo(i);
	// 	averageDistance /= samples.Count;
	// 	return averageDistance;
	// }

	// private Vector3 getLocalPathDirection(List<Vector3> samples)
	// {
	// 	List<Vector3> directions= new List<Vector3>();
	// 	Vector3 averageDirection = new Vector3();

		

	// 	for(int i=0; i < samples.Count - 1; i++)
	// 		directions.Add(samples[i].DirectionTo(samples[i+1]));
		
	// 	foreach(Vector3 i in directions)
	// 		averageDirection += i;

	// 	return averageDirection.Normalized();
	// }
}
