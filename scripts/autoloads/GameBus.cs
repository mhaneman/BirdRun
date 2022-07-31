using Godot;
using System;

public class GameBus : Node
{
	[Signal]public delegate void PlayerDied();
	
	public int eggs = 100;
	public int fruits = 0;
	
}
