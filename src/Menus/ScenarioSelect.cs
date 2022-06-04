using Godot;
using System.Collections.Generic;

namespace Soteria.Menus
{
    public class ScenarioSelect : Control
    {
        // Dictionary of ButtonLabel => ScenarioName
        private readonly Dictionary<string,string> scenarios = new Dictionary<string, string>()
        {
            { "0", "DebugStage" },
            { "1", "Scenario1" },
        };

        private int selectedScenario;

        private GridContainer grid;
        private PackedScene scenarioSelectButtonScene;


        public override void _Ready()
        {
            this.grid = this.GetNode<GridContainer>("VBoxContainer/Control/GridContainer");
            this.scenarioSelectButtonScene = (PackedScene)ResourceLoader.Load("res://Menus/ScenarioSelectButton.tscn");
            foreach(var scenario in this.scenarios) {
                this.createButton(scenario);
            }
        }

        private void createButton(KeyValuePair<string, string> scenario)
        {
            var button = new Button();
            button.Text = scenario.Key;
            button.Name = scenario.Key;
            button.RectMinSize = new Vector2(100,100);
            button.Connect("pressed", this, nameof(this.switchToScene), new Godot.Collections.Array() { scenario.Value } );
            this.grid.AddChild(button);
        }

        private void switchToScene(string scenario_name)
        {
            this.GetTree().ChangeScene("Scenarios/" + scenario_name + "/" + scenario_name + ".tscn");
        }
    }
}