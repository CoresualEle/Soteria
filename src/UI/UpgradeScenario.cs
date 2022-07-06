using Godot;

namespace Soteria.UI
{
    public class UpgradeScenario : Control
    {
        private GameVariables gameVariables;
        private Button nextScenarioButton;

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            this.nextScenarioButton = this.GetNode<Button>("NextScenarioButton");

            this.nextScenarioButton.Disabled = true;

            this.gameVariables.Connect(nameof(GameVariables.BudgetChanged), this, nameof(this.OnBudgetChanged));

            // Normally we would also need to check current infections, but for
            // now we assume that the function will be called anyway on the next day

            this.nextScenarioButton.Connect("pressed", this, nameof(this.DoUpgrade));
        }

        public void OnScenarioLoaded()
        {
            var costToUpgrade = this.gameVariables.CostToUpgrade;
            this.nextScenarioButton.HintTooltip = $"Upgrade to next scenario (Costs {costToUpgrade}€) - Only available when there are no active Infections";
        }

        private void OnBudgetChanged(int budget)
        {
            if (budget >= this.gameVariables.CostToUpgrade && this.gameVariables.CurrentInfections == 0)
            {
                // Only enable the button if it wasn't enabled previously
                if (this.nextScenarioButton.Disabled)
                {
                    this.nextScenarioButton.Disabled = false;
                }
            }
            else if (!this.nextScenarioButton.Disabled)
            {
                this.nextScenarioButton.Disabled = true;
            }
        }

        private void DoUpgrade()
        {
            GD.Print("Upgraded Scenario");

            // TODO actually load next scenario
        }
    }
}