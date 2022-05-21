using Godot;
using Soteria;

public class ContextMenuBoolean : VBoxContainer
{
    [Export]
    public string OptionLabel;

    [Export]
    public int Cost;

    [Export]
    public int Upkeep;

    [Signal]
    public delegate void ButtonToggled(bool value);

    [Export]
    public bool Enabled { get; set; }

    public override void _Ready()
    {
        this.GetNode<Label>("Name").Text = this.OptionLabel;
        this.GetNode<Label>("CostBox/CostValue").Text = this.Cost.ToString();
        this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = this.Upkeep.ToString();
        this.GetNode<CheckButton>("CheckButton").Pressed = this.Enabled;
    }

    private void _on_CheckButton_toggled(bool value)
    {
        this.EmitSignal(nameof(ButtonToggled), value);
        var gameVariables = GetNode<GameVariables>("/root/GameVariables");
        if (value)
        {
            gameVariables.Budget -= this.Cost;
            gameVariables.Upkeep += this.Upkeep;
        } else
        {
            gameVariables.Upkeep -= this.Upkeep;
        }
    }
}