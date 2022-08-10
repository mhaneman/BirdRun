using Godot;
using System;
using System.Threading;

using System.Collections.Generic;

public class PooledObject<T> where T : Spatial
{
	private Transform discardLoc = new Transform();
	private LinkedList<T> working = new LinkedList<T>(); // eventually make double linked list
	private Stack<T> retired = new Stack<T>();
	
	private Spatial Other;
	public string ScenePath;
	
	public T PeekWorking()
	{
		if (working.Count > 0)
			return working.Last.Value;
		return null;
	}
	
	public PooledObject(Spatial Other, string ScenePath, int InitCount=1) 
	{
		this.discardLoc.origin = new Vector3(0, -20, 0);
		this.Other = Other;
		this.ScenePath = ScenePath;

		DefferedInstance(InitCount); 
	}
	
	private void DefferedInstance(int count=1) 
	{
		for (int i=0; i<count; i++)
		{
			PackedScene scene = (PackedScene)ResourceLoader.Load(ScenePath);
			T t = scene.Instance<T>();
			Other.CallDeferred("add_child", t);
			t.GlobalTransform = discardLoc;
			t.Visible = false;
			retired.Push(t);
		}
	}

	private void RunningInstance(int count=1)
	{
		for (int i=0; i<count; i++)
		{
			PackedScene scene = (PackedScene)ResourceLoader.Load(ScenePath);
			T t = scene.Instance<T>();
			Other.AddChild(t);
			t.GlobalTransform = discardLoc;
			t.Visible = false;
			retired.Push(t);
		}

	}
	
	private T MakeActive() 
	{
		T t;
		if (retired.Count <= working.Count)
			RunningInstance(working.Count / 2);
		t = retired.Pop();
		t.Visible = true;
		return t;
	}
	
	public T Summon(Transform transform) {
		T t = MakeActive();
		t.GlobalTransform = transform;
		working.AddLast(t);

		return t;
	}
	
	public T Summon(Transform transform, float rotation)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		working.AddLast(t);

		return t;
	}

	public T Summon(Transform transform, float rotation, Vector3 scale)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		t.Scale = scale;
		working.AddLast(t);

		return t;
	}

	// borrow a retired platform to get global transforms of all connectors
	public List<(string, Transform)> getConnectors(string childName, Transform transform, float rotation = 0f)
	{
		var children = new List<(string, Transform)>();
		T t = retired.Peek();
		t.GlobalTransform = transform;
		t.RotateY(rotation);

		Spatial connector = t.GetNode<Spatial>(childName);
		foreach(Spatial n in connector.GetChildren())
			children.Add((n.Name, n.GlobalTransform));

		t.GlobalTransform = discardLoc;
		return children;
	}

	// borrow a retired platform to get global transform of a specific connector
	public Vector3 getConnector(string childName, Transform transform, float rotation = 0f)
	{
		T t = retired.Peek();
		t.GlobalTransform = transform;
		t.RotateY(rotation);

		Vector3 connector = t.GetNode<Spatial>(childName).GlobalTransform.origin;

		t.GlobalTransform = discardLoc;
		return connector;
	}
	
	public void Dismiss() 
	{
		if (working.Count == 0)
			return;
		
		T t = working.First.Value;
		working.RemoveFirst();
		
		t.GlobalTransform = discardLoc;
		t.Visible = false;
		retired.Push(t);
	}
	
	public void Clear()
	{
		
		while(working.Count > 0)
			this.Dismiss();
			
		/* GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath); */
	}
}