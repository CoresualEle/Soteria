using Godot;
using Soteria;
using System;

public class Stats : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var gameVariables = GetNode<GameVariables>("/root/GameVariables");
		gameVariables.Connect(nameof(GameVariables.BudgetChanged), this, nameof(OnBudgetChanged));
		gameVariables.Connect(nameof(GameVariables.UpkeepChanged), this, nameof(OnUpkeepChanged));

		this.OnBudgetChanged(gameVariables.Budget);
		this.OnUpkeepChanged(gameVariables.Upkeep);
	}

	private void OnBudgetChanged(int budget)
	{
		var budgetValueLabel = GetNode<Label>("Background/BudgetValueLabel");
		budgetValueLabel.Text = budget.ToString();
	}

	private void OnUpkeepChanged(int upkeep)
	{
		var upkeepValueLabel = GetNode<Label>("Background/UpkeepValueLabel");
		upkeepValueLabel.Text = upkeep.ToString();
	}
}
