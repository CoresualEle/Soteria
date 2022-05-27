using Godot;

namespace Soteria.UI
{
    public class Stats : Control
    {
        public override void _Ready()
        {
            var gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            gameVariables.Connect(nameof(GameVariables.BudgetChanged), this, nameof(this.OnBudgetChanged));
            gameVariables.Connect(nameof(GameVariables.UpkeepChanged), this, nameof(this.OnUpkeepChanged));

            this.OnBudgetChanged(gameVariables.Budget);
            this.OnUpkeepChanged(gameVariables.Upkeep);
        }

        private void OnBudgetChanged(int budget)
        {
            var budgetValueLabel = this.GetNode<Label>("Background/BudgetValueLabel");
            budgetValueLabel.Text = budget.ToString();
        }

        private void OnUpkeepChanged(int upkeep)
        {
            var upkeepValueLabel = this.GetNode<Label>("Background/UpkeepValueLabel");
            upkeepValueLabel.Text = upkeep.ToString();
        }
    }
}