using Godot;

namespace Soteria.Menus
{
    public class ScenarioSelect : Control
    {
        private readonly string[] scenarios =
        {
            "Scenario1",
            "DebugStage"
        };

        private int selectedScenario;

        public override void _Ready()
        {
            this.select_scenario(0);
        }

        private void select_scenario(int number)
        {
            int Mod(int x, int m) => (x % m + m) % m; // Modulus of negative numbers: https://stackoverflow.com/a/1082938

            this.selectedScenario = Mod(number, this.scenarios.Length);
            var scenarioNameLabel = this.GetNode<Label>("HBoxContainer/VBoxContainer/ScenarioName");
            scenarioNameLabel.Text = this.scenarios[this.selectedScenario];
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_left"))
            {
                this._on_PreviousScenarioButton_pressed();
            }

            if (@event.IsActionPressed("ui_right"))
            {
                this._on_NextScenarioButton_pressed();
            }
        }

        private void _on_PreviousScenarioButton_pressed()
        {
            this.select_scenario(this.selectedScenario - 1);
        }

        private void _on_NextScenarioButton_pressed()
        {
            this.select_scenario(this.selectedScenario + 1);
        }

        private void _on_StartScenarioButton_pressed()
        {
            var scenarioName = this.scenarios[this.selectedScenario];
            this.GetTree().ChangeScene("res://Scenarios/" + scenarioName + "/" + scenarioName + ".tscn");
        }
    }
}