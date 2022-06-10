using Godot;

public class GameOver : Control
{
    [Signal]
    public delegate void RestartButtonPressed();

    public override void _Ready()
    {
        this.GetNode<Button>("VBoxContainer/RestartButton").GrabFocus();
    }

    private void _on_RestartButton_pressed()
    {
        this.EmitSignal(nameof(RestartButtonPressed));
    }

    private void _on_BackToScenarioSelectionButton_pressed()
    {
        this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
    }
}
