using Godot;
using Soteria;

public class GameOver : Control
{
    public override void _Ready()
    {
        this.GetNode<Button>("VBoxContainer/RestartButton").GrabFocus();
    }

    private void _on_RestartButton_pressed()
    {
        var gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
        var scenarioName = gameVariables.CurrentScenarioName;
        this.GetTree().ChangeScene("Scenarios/" + scenarioName + "/" + scenarioName + ".tscn");
    }

    private void _on_BackToScenarioSelectionButton_pressed()
    {
        this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
    }

    private void _on_BackToMainMenuButton_pressed()
    {
        this.GetTree().ChangeScene("res://Menus/Main.tscn");
    }
}
