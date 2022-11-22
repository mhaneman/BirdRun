using Godot;
using System;

public class CharacterControl : Spatial
{
    private AnimationPlayer ap;
    public override void _Ready()
    {
        ap = GetNode<AnimationPlayer>("AnimationPlayer");
        ap.Play("running");
    }
}
