using Godot;
using System;

using System.Collections.Generic;

public class Chunk<T> : Node where T : Spatial
{
	/*
	the linked list of platforms defines the chunk 
	*/
	public LinkedList<T> Order = new LinkedList<T>();
	public Dictionary<string, PooledObject<T>> PlatformTypes = new Dictionary<string, PooledObject<T>>();
	
	private Spatial Other;
	public Chunk(Spatial Other)
	{
		this.Other = Other;
		this.AddPlatformType("res://scenes/platforms/Connector.tscn", "Connector", 30);
		this.AddPlatformType("res://scenes/platforms/Flat.tscn", "Flat", 50);
		this.AddPlatformType("res://scenes/platforms/Upright.tscn", "Upright", 10);
		this.AddPlatformType("res://scenes/platforms/Stair.tscn", "Stair", 50);
		this.AddPlatformType("res://scenes/platforms/Gap.tscn", "Gap", 10);
	}
	
	public void AddPlatformType(string ScenePath, string name, int InitCount=1)
	{
		PlatformTypes.Add(name, new PooledObject<T>(Other, ScenePath, InitCount));
	}
	
	public void AddNext(string PlatformType)
	{
		Transform SpawnLoc;
		if (Order.Count > 0)
			SpawnLoc = Order.First.Value.GetNode<Spatial>("Connectors/Back").GlobalTransform;
		else 
			SpawnLoc = Other.GlobalTransform;
			
		Order.AddFirst(PlatformTypes[PlatformType].Summon(SpawnLoc));
	}
	
	public void AddNext(string PlatformType, int num)
	{
		for (int i=0; i<num; i++)
			this.AddNext(PlatformType);
	}
	
	public void AddNext(string PlatformType, string Direction)
	{
		float Rotation;
		string Connector;
		
		if (Direction == "left")
		{
			Rotation = -Mathf.Pi/2;
			Connector = "Connectors/Left";
		} 
		else if (Direction == "right")
		{
			Rotation = Mathf.Pi/2;
			Connector = "Connectors/Right";
		} 
		else
		{
			Rotation = 0f;
			Connector = "Connectors/Back";
		}
		
		Transform SpawnLoc;
		if (Order.Count > 0) 
		{
			SpawnLoc = Order.First.Value.GetNode<Spatial>(Connector).GlobalTransform;
		}
		else 
		{
			SpawnLoc = Other.GlobalTransform;	
		}
		Order.AddFirst(PlatformTypes[PlatformType].Summon(SpawnLoc, Rotation));
	}
	
	public void RemoveLast()
	{
		
	}
	
	
}
