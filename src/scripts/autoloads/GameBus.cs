using Godot;


// this class is used for public signals;
public class GameBus : Node
{
	[Signal]public delegate void PlayerDied();
	[Signal]public delegate void EnteredPortal();
	[Signal]public delegate void OnFloor();

	public bool IsOnFloor = false;
}
