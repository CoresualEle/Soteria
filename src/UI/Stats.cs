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
            gameVariables.Connect(nameof(GameVariables.IncomeChanged), this, nameof(this.OnIncomeChanged));

            this.OnBudgetChanged(gameVariables.Budget);
            this.OnUpkeepChanged(gameVariables.Upkeep);
            this.OnIncomeChanged(gameVariables.BaseIncome);
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

        private void OnIncomeChanged(int income)
        {
            var incomeValueLabel = this.GetNode<Label>("Background/IncomeValueLabel");
            incomeValueLabel.Text = income.ToString();
        }
    }
}
