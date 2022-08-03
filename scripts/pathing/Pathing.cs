using Godot;
using System;
using System.Collections.Generic;


public class Pathing
{
	private AStarPathingStrategy strategy = new AStarPathingStrategy();
	private Vector3 startPos = new Vector3(2, 0, 2);
	private Vector3 goalPos = new Vector3(12, 0, 12);

	private static float withinReachRange = 1f;


	public LinkedList<Vector3> findGoal() 
	{
		return strategy.computePath(startPos, goalPos, withinReach, potentialNeighbors);
	}
   Func<Vector3, List<Vector3>> potentialNeighbors = Point =>
   {
	   List<Vector3> n = new List<Vector3>();
	   n.Add(new Vector3(Point.x - 1, Point.y, Point.z));
	   n.Add(new Vector3(Point.x + 1, Point.y, Point.z));
	   n.Add(new Vector3(Point.x, Point.y, Point.z - 1));
	   n.Add(new Vector3(Point.x, Point.y, Point.z + 1));
	   return n;
   }; 

   Func<Vector3, Vector3, bool> withinReach = (Current, End) => 
   {
	   if (Mathf.Abs(Current.x - End.x) < withinReachRange && 
	   	Mathf.Abs(Current.y - End.y) < withinReachRange && 
		Mathf.Abs(Current.z - End.z) < withinReachRange)
			return true;
		return false;
   }; 
}
