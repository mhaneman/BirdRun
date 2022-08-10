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
		Connector = new PooledObject<T>(Other, "res://scenes/platforms/Connector.tscn", 10);

		this.AddPlatformType("res://scenes/platforms/Flat.tscn", 10);
		this.AddPlatformType("res://scenes/platforms/Stair.tscn", 10);
		this.AddPlatformType("res://scenes/platforms/Down.tscn", 10);
		this.AddPlatformType("res://scenes/platforms/Gap.tscn", 10);
		//this.AddPlatformType("res://scenes/platforms/Upright.tscn", 10);
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

	public void Summon(int platformType, float rotation, int count)
	{
		for(int i=0; i<count; i++)
			Summon(platformType, rotation);
	}

	public void Summon(int platformType, float rotation, Vector3 scale)
	{
		Transform spawnLoc = getNextEndPoint(rotation);
		ActivePlatforms.AddFirst(PlatformTypes[platformType].Summon(spawnLoc, rotation, scale));
	}

	public void Summon(int platformType, float rotation, Vector3 scale, int count)
	{
		for(int i=0; i<count; i++)
			Summon(platformType, rotation, scale);
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
			var connectors = Connector.getConnectors("Connectors", currentPoint);
			foreach((string name, Transform pos) s in connectors)
			{
				float rotation = 0f;
				if (s.name == "Left")
					rotation = -Mathf.Pi/2;

				if (s.name == "Right")
					rotation = Mathf.Pi/2;

				Vector3 end = PlatformTypes[platformNum].getConnector("Connectors/Back", s.pos, rotation);
				points.Add((platformNum, rotation), end);
			}
		}
		return points;
	}
}