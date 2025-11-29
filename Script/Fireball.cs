using Godot;
using System;

public partial class Fireball : CharacterBody2D
{
	public Node2D TargetNode;
	public int projectileDamage = 5;	
	private int speed = 50;
	
	public override void _PhysicsProcess(double delta) 
	{
		if (TargetNode == null || !IsInstanceValid(TargetNode))
		{
			QueueFree();
			return;	
		}
		
		Vector2 targetPosition = TargetNode.GlobalPosition;
		Vector2 direction = (targetPosition - GlobalPosition).Normalized();
		Velocity = direction * speed;
		
		LookAt(targetPosition);
		
		MoveAndSlide();
	}
	
	private void _on_area_2d_body_entered(Node body) 
	{
		GD.Print(GlobalPosition);
		if (body is Malware m) 
		{
			m.Health -= projectileDamage;
			QueueFree();
		}
	}
}
