using System.Collections.Generic;

using Godot;
using Godot.Collections;

namespace Soteria.Menus
{
    public class ScenarioSelect : Control
    {
        // Dictionary of ButtonLabel => ScenarioName
        private readonly System.Collections.Generic.Dictionary<string, string> scenarios = new System.Collections.Generic.Dictionary<string, string>
        {
            { "0", "DebugStage" },
            { "1", "Scenario1" },
        };

        private GridContainer grid;

        public override void _Ready()
        {
            this.grid = this.GetNode<GridContainer>("VBoxContainer/Control/GridContainer");

            foreach (var scenario in this.scenarios)
            {
                this.CreateButton(scenario);
            }
        }

        private void CreateButton(KeyValuePair<string, string> scenario)
        {
            var button = new Button();
            button.Text = scenario.Key;
            button.Name = scenario.Key;
            button.RectMinSize = new Vector2(100, 100);
            button.Connect(nameof(BaseButton._Pressed) this, nameof(this.SwitchToScene), new Array { scenario.Value });
            this.grid.AddChild(button);
        }

        private void SwitchToScene(string scenarioName)
        {
            this.GetTree().ChangeScene("Scenarios/" + scenarioName + "/" + scenarioName + ".tscn");
        }
    }
}