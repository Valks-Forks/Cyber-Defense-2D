using Godot;
using System;
using System.Collections.Generic;

public partial class FirewallTower : StaticBody2D
{
	private PackedScene bullet = GD.Load<PackedScene>("res://Sprite/projectile/fireball.tscn");
	private int bulletDamage = 5;

	private List<Node> currentTargets = new();
	private Node2D currentTarget;

	private bool projectileInAir = false;

	public override void _Process(double delta)
	{
		currentTargets.RemoveAll(n => n == null || !IsInstanceValid(n));

		if (currentTarget == null || !IsInstanceValid(currentTarget))
			SelectTarget();

		if (currentTarget != null && !projectileInAir)
			FireProjectile();
	}

	private void SelectTarget()
	{
		currentTarget = null;

		foreach (var node in currentTargets)
		{
			if (node is Malware m)
			{
				currentTarget = m;
				break;
			}
			else if (node is PathFollow2D pf && pf.GetChildCount() > 0 && 
					 pf.GetChild(0) is Malware childM)
			{
				currentTarget = childM;
				break;
			}
		}
	}

	private void FireProjectile()
	{
		projectileInAir = true; 

		Fireball proj = (Fireball)bullet.Instantiate();
		proj.TargetNode = currentTarget;
		proj.projectileDamage = bulletDamage;

		Marker2D aim = GetNode<Marker2D>("Aim");
		proj.GlobalPosition = aim.GlobalPosition;
		proj.LookAt(currentTarget.GlobalPosition);

		GetNode("FireballContainer").CallDeferred("add_child", proj);

		proj.Connect("tree_exited", Callable.From(() =>
		{
			projectileInAir = false;
		}));
	}

	private void _on_tower_body_entered(Node body)
	{
		if (body is Malware && !currentTargets.Contains(body))
			currentTargets.Add(body);
	}

	private void _on_tower_body_exited(Node body)
	{
		if (currentTargets.Contains(body))
			currentTargets.Remove(body);
			
		if (body == currentTarget)
			currentTarget = null;
	}
}
