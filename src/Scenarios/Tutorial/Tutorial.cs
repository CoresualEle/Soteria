using Soteria.UI;

namespace Soteria.Scenarios.Tutorial
{
    public class Tutorial : ScenarioBase
    {
        public override void _Ready()
        {
            base._Ready(); // First invoke base, because this is where the gameVariables are loaded

            this.GameVariables.BaseIncome = 2000;
            this.GameVariables.Budget = 20000;
            this.GameVariables.CostToUpgrade = 25000;
            this.GameVariables.WorkSatisfaction = 0.8f;

            this.EmitSignal(nameof(ScenarioLoaded));
            this.GameVariables.SetTimeScale(0);

            var tutorialOverlay = this.GetNode<TutorialOverlay>("TutorialOverlay");
            tutorialOverlay.Connect(nameof(TutorialOverlay.TutorialDone), this, nameof(this.BackToScenarioSelection));
        }

        private void BackToScenarioSelection()
        {
            this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
        }
    }
}