using Godot;
using System;
using System.Collections.Generic;

public class ScenarioSelect : Control
{
    private int selected_scenario;
    private string[] scenarios =
    {
        "Scenario1",
        "DebugStage"
    };

    public override void _Ready()
    {
        this.select_scenario(0);
    }

    private void select_scenario(int number)
    {
        int mod(int x, int m) {
            // Modulus of negative numbers
            // https://stackoverflow.com/a/1082938
            return (x%m + m)%m;
        }
        this.selected_scenario = mod(number, this.scenarios.Length);
        GD.Print(this.selected_scenario);
        var scenario_name_label = this.GetNode<Label>("HBoxContainer/VBoxContainer/ScenarioName");
        scenario_name_label.Text = this.scenarios[this.selected_scenario];
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
        this.select_scenario(this.selected_scenario - 1);
    }
    private void _on_NextScenarioButton_pressed()
    {
        this.select_scenario(this.selected_scenario + 1);
    }
    private void _on_StartScenarioButton_pressed()
    {
        var scenario_name = this.scenarios[this.selected_scenario];
        this.GetTree().ChangeScene("res://Scenarios/"+scenario_name+"/"+scenario_name+".tscn");
    }

}
