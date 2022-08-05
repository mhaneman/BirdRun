using Godot;
using System;

public class GameBus : Node
{
	[Signal]public delegate void PlayerDied();
	
	public int eggs = 3;
	public int fruits = 0;
	
}
