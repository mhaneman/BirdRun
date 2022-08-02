using Godot;
using System;
using System.Collections.Generic;

interface PathingStrategy
{
	LinkedList<Vector3> computePath(
		Vector3 start, 
		Vector3 end,
		Func<Vector3, List<Vector3>> potentialNeighbors);

	static Func<Vector3, List<Vector3>> NEIGHBORS = delegate(Vector3 point)
	{
		//should eventually just get these from the platforms themselves
		var n = new List<Vector3>();
		n.Add(new Vector3(point.x - 1, point.y, point.z));
		n.Add(new Vector3(point.x + 1, point.y, point.z));
		n.Add(new Vector3(point.x, point.y, point.z - 1));
		n.Add(new Vector3(point.x, point.y, point.z + 1)); 
		return n;
	};

}

