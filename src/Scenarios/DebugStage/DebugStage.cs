namespace Soteria.Scenarios.DebugStage
{
    public class DebugStage : ScenarioBase
    {
        public override void _Ready()
        {
            base._Ready(); // First invoke base, because this is where the gameVariables are loaded

            this.GameVariables.BaseIncome = 10000;
            this.GameVariables.Budget = 20000;
            this.GameVariables.CostToUpgrade = 50;
            this.GameVariables.WorkSatisfaction = 0.8f;
        }
    }
}
