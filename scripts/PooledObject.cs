using Godot;
using System;

using System.Collections.Generic;

public class PooledObject<T> where T : Spatial
{
	private Transform discardLoc = new Transform();
	private LinkedList<T> working = new LinkedList<T>();
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
		
		for(int i=0; i<InitCount; i++)
		{
			T t = Instance();
			t.GlobalTransform = discardLoc;
			t.Visible = false;
			retired.Push(t);	
		}
	}
	
	private T Instance() 
	{
		PackedScene scene = (PackedScene)ResourceLoader.Load(ScenePath);
		T t = scene.Instance<T>();
		Other.CallDeferred("add_child", t);
		return t;
	}
	
	private T MakeActive() 
	{
		T t;
		if (retired.Count == 0)
			t = Instance();
		else
			t = retired.Pop();
		t.Visible = true;
		return t;
	}
	
	public T Summon(Transform transform) {
		T t = MakeActive();
		t.GlobalTransform = transform;
		working.AddLast(t);

		GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath);
			
		return t;
	}
	
	public T Summon(Transform transform, float rotation)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		working.AddLast(t);

		GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath);

		return t;
	}

	public T Summon(Transform transform, float rotation, Vector3 scale)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		t.Scale = scale;
		working.AddLast(t);

		GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath);

		return t;
	}

	public T PeekFromRetired(Transform transform)
	{
		T t = retired.Peek();
		t.GlobalTransform = transform;
		return t;
	}

	public T PeekFromRetired(Transform transform, float rotation)
	{
		T t = retired.Peek();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		return t;
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
	
	public void DismissAll()
	{
		while(working.Count > 0)
			this.Dismiss();	
	}
}
