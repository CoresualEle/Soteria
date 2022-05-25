using Godot;
using Soteria;
using System;

public class Stats : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().Root.Connect("size_changed", this, nameof(Resize));
		Resize();

		var gameVariables = GetNode<GameVariables>("/root/GameVariables");
		gameVariables.Connect(nameof(GameVariables.BudgetChanged), this, nameof(OnBudgetChanged));
		gameVariables.Connect(nameof(GameVariables.UpkeepChanged), this, nameof(OnUpkeepChanged));
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

	private void Resize()
	{
		var background = GetNode<Polygon2D>("Background");
		var visibleRect = GetTree().Root.GetVisibleRect();
		var size = visibleRect.Size;

		background.Position = new Vector2(0, size.y - 50);

		var polygon = background.Polygon;
		polygon[2].x = size.x;
		polygon[3].x = size.x;
		background.Polygon = polygon;
	}
}
