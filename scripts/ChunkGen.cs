using Godot;
using System;

public class ChunkGen : Spatial
{
	private Chunk<StaticBody> chunk;
	public override void _Ready()
	{
		chunk = new Chunk<StaticBody>(this);
	}
	
	private int i = 0;
	public override void _Process(float delta)
	{
			
		if (i < 16)
		{
			if (i < 1)
			{
				chunk.AddNext("Flat", 15);
				chunk.AddNext("Upright");
				chunk.AddNext("Flat", 3);
				chunk.AddNext("Gap");
				chunk.AddNext("Flat", 3);
				chunk.AddNext("Connector");
				chunk.AddNext("Flat", "left");
				chunk.AddNext("Flat", 2);
				
			}
			else if (i < 5)
			{
				chunk.AddNext("Connector");
				chunk.AddNext("Stair", "right");
				chunk.AddNext("Stair");
			}
			else if (i < 10)
			{
				chunk.AddNext("Connector");
				chunk.AddNext("Stair", "left");
			} else if (i < 15)
			{
				chunk.AddNext("Connector");
				chunk.AddNext("Stair", "right");
				chunk.AddNext("Stair", 2);
				chunk.AddNext("Gap");
			} else
			{
				chunk.AddNext("Connector");
				chunk.AddNext("Gap");
				chunk.AddNext("Flat", 2);
				chunk.AddNext("Connector");
				chunk.AddNext("Flat", "right");
				chunk.AddNext("Connector");
				chunk.AddNext("Flat", "right");
				chunk.AddNext("Connector");
				chunk.AddNext("Upright", "right");
				chunk.AddNext("Connector");
				chunk.AddNext("Flat");
				chunk.AddNext("Gap");
				chunk.AddNext("Stair", 5);
				
			}
			i++;
		}
	}
}
