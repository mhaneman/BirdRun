using Godot;
using System;
using System.Collections.Generic;

public class PathingMain
{
	private LinkedList<Vector3> path;
	private PathingStrategy strategy = new AStarPathingStrategy();
	private Vector3 startPos = new Vector3(2, 0, 2);
	private Vector3 goalPos = new Vector3(14, 0, 13);
	private bool foundPath = false;


	private LinkedList<Vector3> findGoal(Vector3 pos, Vector3 goal, List<Vector3> path) 
	{
		LinkedList<Vector3> points = strategy.computePath(pos, goalPos, PathingStrategy.NEIGHBORS);
		
		if (points.Count == 0) 
			return null;
		
		return points;
   }
}
