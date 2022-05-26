using Godot;
using Soteria;
using System;

public class ContextMenuAction : VBoxContainer
{
    [Export]
    public string OptionLabel;

    [Export]
    public int Cost;

    [Signal]
    public delegate void ActionPressed();


    public override void _Ready()
    {
        this.GetNode<Label>("Name").Text = this.OptionLabel;
        this.GetNode<Label>("CostBox/CostValue").Text = this.Cost.ToString();
    }

    private void _on_ActionButton_pressed()
    {
        this.EmitSignal(nameof(ActionPressed));
        var gameVariables = GetNode<GameVariables>("/root/GameVariables");
        gameVariables.Budget -= this.Cost;
        // TODO maybe disable this button for a certain time to not misclick
    }
}
