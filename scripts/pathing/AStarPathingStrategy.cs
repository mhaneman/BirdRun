using Godot;
using System;
using System.Collections.Generic;

class AStarPathingStrategy : PathingStrategy
{
	public LinkedList<Vector3> computePath(
		Vector3 start, 
		Vector3 end,
		Func<Vector3, List<Vector3>> potentialNeighbors)
	{
		var cameFrom = new Dictionary<Vector3, Vector3>();
		var gScore = new Dictionary<Vector3, int>();
		var fScore = new Dictionary<Vector3, int>();
		var openSet = new PriorityQueue<Vector3, int>();

		openSet.Enqueue(start, fScore[start]);
		gScore.Add(start, 0);
		fScore.Add(start, heuristic(start, end));

		while (openSet.Count > 0)
		{
			Vector3 current = openSet.Dequeue();
			if (current == end)
				return reconstructPath(cameFrom, start, current);


			potentialNeighbors(current).ForEach(i =>
				{
					int tenative_gScore = gScore[current] + 1;
					int neighbor_gScore = gScore.ContainsKey(i) ? gScore[i] : 0;
					if (tenative_gScore < neighbor_gScore || !gScore.ContainsKey(i))
					{
						cameFrom.Add(i, current);
						gScore.Add(i, tenative_gScore);
						fScore.Add(i, tenative_gScore + heuristic(i, end));
							if (!openSet.contains(i))
								openSet.Add(i, fScore[i]);
					}
				}
			);
		}
		return null;
	}

	public LinkedList<Vector3> reconstructPath(Dictionary<Vector3, Vector3> cameFrom, Vector3 start, Vector3 end)
	{
		//should eventually just return a bunch of commands for the chunks to understand
		LinkedList<Vector3> path = new LinkedList<Vector3>();
		Vector3 current = end;
		path.AddFirst(current);
		while(current != start)
		{
			path.AddFirst(cameFrom[current]);
			current = cameFrom[current];
		}
		return path;
	}

	public int heuristic(Vector3 a, Vector3 b)
	{
		return (int) (Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z));
	}
}
