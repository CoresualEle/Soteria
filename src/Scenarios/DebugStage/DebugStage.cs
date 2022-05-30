namespace Soteria.Scenarios.DebugStage
{
    public class DebugStage : ScenarioBase
    {
        public override void _Ready()
        {
            base._Ready(); // First invoke base, because this is where the gameVariables are loaded

            this.GameVariables.Budget = 20000;
            this.GameVariables.WorkSatisfaction = 0.8f;
        }
    }
}