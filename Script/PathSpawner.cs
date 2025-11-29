using Godot;
using System;

public partial class PathSpawner : Node2D
{
	private PackedScene pathScene = GD.Load<PackedScene>("res://Sprite/path/path.tscn");

	public override void _Ready()
	{
		Timer timer = GetNode<Timer>("Timer");
		timer.Timeout += _on_Timer_timeout;
	}

	private void _on_Timer_timeout()
	{
		Node2D tempPath = (Node2D)pathScene.Instantiate();
		AddChild(tempPath);
	}
}
