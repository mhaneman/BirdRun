using Godot;
using System;
using System.Collections.Generic;

public class Music : Node
{
	List<AudioStreamPlayer> audio = new List<AudioStreamPlayer>();
	
	public override void _Ready()
	{
		foreach(AudioStreamPlayer i in this.GetChildren())
		{
			i.Connect("finished", this, "on_Finished");
			audio.Add(i);	
		}
		audio[0].Play();
	}
	
	private void on_Finished()
	{
		audio[0].Play();
	}
}
