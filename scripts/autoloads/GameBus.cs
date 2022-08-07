using Godot;
using System;

public class GameBus : Node
{
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void EnteredPortal();
	
	// move to game control
	public int eggs = 3;
	public int fruits = 0;
}
