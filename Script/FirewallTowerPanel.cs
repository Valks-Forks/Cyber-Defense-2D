using Godot;
using System;

public partial class FirewallTowerPanel : Panel
{	
	private PackedScene towerScene = GD.Load<PackedScene>("res://Sprite/tower/firewall_tower.tscn");
	private PackedScene previewScene = GD.Load<PackedScene>("res://Sprite/tower/firewall_tower_preview.tscn");

	private StaticBody2D preview;
	private bool dragging = false;
	
	private Node2D towersNode;
	private Area2D towerBaseArea;
	
	private const int TOWER_PRICE = 20;
	
	public override void _Ready()
	{
		towersNode = GetTree().CurrentScene.GetNode<Node2D>("Towers");
		towerBaseArea = GetTree().CurrentScene.GetNode<Area2D>("Metallic Tower Base/Area2D");
		
		this.GuiInput += _on_gui_input;
	}
	
	public override void _Process(double delta)
	{
		if (dragging && preview != null)
			preview.GlobalPosition = GetGlobalMousePosition();	
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (!dragging) return;

		if (@event is InputEventMouseButton MB &&
			MB.ButtonIndex == MouseButton.Left &&
			MB.Pressed)
		{
			TowerPlacement();
		}
		
		if (@event is InputEventMouseButton MB2 &&
			MB2.ButtonIndex == MouseButton.Right &&
			MB2.Pressed)
		{
			CancelDragTower();
		}
		
	}	
	
	private void _on_gui_input(InputEvent @event)
	{		
		// Star Drag of Tower
		if (
			@event is InputEventMouseButton MB1 && 
			MB1.ButtonIndex == MouseButton.Left &&
			MB1.Pressed	&& !dragging
		)
		{
			if (!GameManager.Instance.CanAfford(TOWER_PRICE))
			{
				GD.Print("Not enough coints to buy the tower!");
				return;
			}
			
			StartDragTower();
		}
	}

	private void StartDragTower()
	{
		dragging = true;
		
		var shopPanel = GetTree().Root.GetNode<Control>("Main/Shop/Shop");		
		if (shopPanel != null)
			shopPanel.Visible = false;		
		
		preview = towersNode.GetNodeOrNull<StaticBody2D>("FirewallTowerPreview");
		if (preview == null)
		{
			preview = previewScene.Instantiate<StaticBody2D>();
			preview.Name = "FirewallTowerPreview";
			towersNode.AddChild(preview);
		}		
		
		var sprite = preview.GetNode<Sprite2D>("FirewallTower");
		sprite.Modulate = new Color(1, 1, 1, 0.4f);			
	}
	
	private bool IsInsideTowerBase(Vector2 point)
	{
		var space = GetWorld2D().DirectSpaceState;

		var result = space.IntersectPoint(new PhysicsPointQueryParameters2D
		{
			Position = point,
			CollideWithAreas = true
		});

		foreach (var hit in result)
		{
			Node collider = hit["collider"].As<Node>();
			if (collider == towerBaseArea) return true;
		}

		return false;
	}
	
	private void TowerPlacement()
	{	
		Vector2 mousePosition = GetGlobalMousePosition();
		
		if (!IsInsideTowerBase(mousePosition))
		{
			GD.Print("Cannot place tower here! Must be on Metallic Tower Base.");
			return;
		}				
			
		dragging = false;
		
		if (!GameManager.Instance.CanAfford(TOWER_PRICE))
		{
			GD.Print("Not enough coins to place the tower!");
			CancelDragTower();
			return;			
		}
		
		GameManager.Instance.SpendCoins(TOWER_PRICE);		
		
		if (preview.GetParent() != null)
			preview.GetParent().RemoveChild(preview); 
		
		if (preview != null)
		{
			preview.QueueFree();
			preview = null;
		}
		
		FirewallTower tower = towerScene.Instantiate<FirewallTower>();
		tower.GlobalPosition = mousePosition;
		
		towersNode.AddChild(tower);
	}
	
	private void CancelDragTower()
	{
		dragging = false;
		
		if (preview.GetParent() != null)
			preview.GetParent().RemoveChild(preview); 		
		
		if (preview != null)
		{
			preview.QueueFree();
			preview = null;
		}
	}
}
