using Godot;
using System;

using System.Collections.Generic;

public class PathSpawner<T> : Node where T : Spatial
{
	public PooledObject<T> Connector;
	public PooledObject<T> Portal;
	public List<PooledObject<T>> PlatformTypes = new List<PooledObject<T>>();
	public LinkedList<T> ActivePlatforms = new LinkedList<T>();
	
	private Spatial Other;
	public PathSpawner(Spatial Other)
	{
		this.Other = Other;
		Portal = new PooledObject<T>(Other, "res://scenes/platforms/Portal.tscn", 5);
		Connector = new PooledObject<T>(Other, "res://scenes/platforms/Connector.tscn", 50);
		this.AddPlatformType("res://scenes/platforms/Flat.tscn", 50);
		this.AddPlatformType("res://scenes/platforms/Stair.tscn", 50);
		this.AddPlatformType("res://scenes/platforms/Down.tscn", 50);
		this.AddPlatformType("res://scenes/platforms/Gap.tscn", 50);

		
		//this.AddPlatformType("res://scenes/platforms/Upright.tscn", 50);
	}
	
	public void AddPlatformType(string ScenePath, int InitCount=1)
	{
		PlatformTypes.Add(new PooledObject<T>(Other, ScenePath, InitCount));
	}
	
	public void Reset()
	{
		ActivePlatforms.Clear();
		Portal.Clear();
		Connector.Clear();
		foreach (var t in PlatformTypes)
			t.Clear();
	}

	public void Summon(int platformType, float rotation)
	{
		Transform spawnLoc = getNextEndPoint(rotation);
		ActivePlatforms.AddFirst(PlatformTypes[platformType].Summon(spawnLoc, rotation));
	}

	public void Summon(int platformType, float rotation, Vector3 scale)
	{
		Transform spawnLoc = getNextEndPoint(rotation);
		ActivePlatforms.AddFirst(PlatformTypes[platformType].Summon(spawnLoc, rotation, scale));
	}
	
	public void SummonPortal()
	{
		Transform spawnLoc = getCurrentEndNode();
		Portal.Summon(spawnLoc);
	}

	private Transform getNextEndPoint(float rotation)
	{
		Transform spawnLoc = getCurrentEndNode();
		Transform c;
		if (rotation == 0f)
			c = spawnLoc;
		else if (rotation <= -Mathf.Pi/2)
			c = Connector.Summon(spawnLoc).GetNode<Spatial>("Connectors/Left").GlobalTransform;
		else if (rotation >= Mathf.Pi/2)
			c = Connector.Summon(spawnLoc).GetNode<Spatial>("Connectors/Right").GlobalTransform;
		else 
			c = Connector.Summon(spawnLoc).GetNode<Spatial>("Connectors/Back").GlobalTransform;
		return c;
	}

	private Transform getCurrentEndNode()
	{
		if (ActivePlatforms.Count > 0) 
			return ActivePlatforms.First.Value.GetNode<Spatial>("Connectors/Back").GlobalTransform;
		return Other.GlobalTransform;	
	}

	public Dictionary<(int, float), Vector3> getAllPlatformConnectingGlobalPoints()
	{
		Transform currentPoint = getCurrentEndNode();
		Dictionary<(int, float), Vector3> points = new Dictionary<(int, float), Vector3>();

		for (int platformNum=0; platformNum<PlatformTypes.Count; platformNum++)
		{
			Spatial n = Connector.PeekFromRetired(currentPoint).GetNode<Spatial>("Connectors");
			foreach(Spatial s in n.GetChildren())
			{
				float rotation = 0f;
				if (s.Name == "Left")
					rotation = -Mathf.Pi/2;

				if (s.Name == "Right")
					rotation = Mathf.Pi/2;
				
				T connectorPoints = PlatformTypes[platformNum].PeekFromRetired(s.GlobalTransform, rotation);
				Vector3 end = connectorPoints.GetNode<Spatial>("Connectors/Back").GlobalTransform.origin;
				points.Add((platformNum, rotation), end);
			}
		}
		return points;
	}
}
