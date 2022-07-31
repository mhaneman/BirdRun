using Godot;
using System;

using System.Collections.Generic;

public class PooledObject<T> where T : Spatial
{
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
		this.Other = Other;
		this.ScenePath = ScenePath;
		
		for(int i=0; i<InitCount; i++)
		{
			T t = Instance();
			
			// disable collision
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
			
		// enable collision
		t.Visible = true;
		return t;
	}
	
	public T Summon(Transform transform) {
		T t = MakeActive();
		t.GlobalTransform = transform;
		working.AddLast(t);
		return t;
	}
	
	public T Summon(Transform transform, Vector3 scale)
	{
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
	
	public T Summon(Transform transform, Vector3 Scale, float rotation)
	{
		T t = MakeActive();
		t.GlobalTransform = transform;
		t.RotateY(rotation);
		working.AddLast(t);
		return t;
		
	}
	
	public void Dismiss() 
	{
		if (working.Count == 0)
			return;
		
		T t = working.First.Value;
		working.RemoveFirst();
		
		// disable collision
		t.Visible = false;
		retired.Push(t);
		
		/* GD.Print("working: ", working.Count, " retired: ", 
			retired.Count, " ", ScenePath); */
	}
	
	public void DismissAll()
	{
		while(working.Count > 0)
			this.Dismiss();	
	}
}
