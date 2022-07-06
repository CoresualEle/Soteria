namespace Soteria.Scenarios.Scenario1
{
    public class Scenario1 : ScenarioBase
    {
        public override void _Ready()
        {
            base._Ready(); // First invoke base, because this is where the gameVariables are loaded

            this.GameVariables.BaseIncome = 2000;
            this.GameVariables.Budget = 20000;
            this.GameVariables.CostToUpgrade = 25000;
            this.GameVariables.WorkSatisfaction = 0.8f;

            this.EmitSignal(nameof(ScenarioLoaded));
        }
    }
}