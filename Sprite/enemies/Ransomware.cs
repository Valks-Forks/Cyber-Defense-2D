using Godot;
using System;

public partial class Ransomware : CharacterBody2D
{
	public const float Speed = 30.0f;

	public override void _PhysicsProcess(double delta)
	{
		var parentNode = GetParent() as PathFollow2D;
		if (parentNode != null)
		{
			parentNode.Progress += Speed * (float)delta;
			
			var path = parentNode.GetParent() as Path2D;
			if (path != null) 
			{
				if (parentNode.Progress >= path.Curve.GetBakedLength()) 
				{
					QueueFree();
				}		
			}
		}
	}
}
