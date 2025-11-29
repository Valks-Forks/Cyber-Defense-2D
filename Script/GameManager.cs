using Godot;
using System;

public partial class GameManager : Node2D
{
	public static GameManager Instance;

	public int Coins = 100;
	public int Health = 100;
	public int startWave;
	public int endWave;

	private Label _coinsLabel;
	private Label _healthLabel;
	private Label _waveLabel;

	public override void _Ready()
	{
		Instance = this;
		
		_coinsLabel = GetNode<Label>("UI/Panel/CoinsLabel");
		_healthLabel = GetNode<Label>("UI/Panel/HealthLabel");
		_waveLabel = GetNode<Label>("UI/Panel/WaveLabel");

		UpdateUI();
	}
	
	public bool CanAfford(int cost)
	{
		return Coins >= cost;
	}	
	
	public void AddCoins(int amount)
	{
		Coins += amount;
		UpdateUI();
	}
		
	public void SpendCoins(int amount)
	{
		Coins -= amount;
		UpdateUI();
	}	

	public void ReduceHealth(int amount)
	{
		Health -= amount;
		if (Health < 0)
			Health = 0;

		UpdateUI();
	}
	
	public void SetWave(int startWave, int endWave)
	{
		 
		
		UpdateUI();
	}
	
	public void GetWave(int startWave, int endWave)
	{
		
		
		UpdateUI();
	}	

	private void UpdateUI()
	{
		_coinsLabel.Text = $"Coins: {Coins}";
		_healthLabel.Text = $"Health: {Health}%";
		_waveLabel.Text = $"Wave: {startWave} / {endWave}";
	}	
}
