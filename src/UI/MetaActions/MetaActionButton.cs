using Godot;

namespace Soteria.UI.MetaActions
{
    public class MetaActionButton : VBoxContainer
    {
        [Export]
        public string ActionName;

        [Export]
        public int Cost;

        private GameVariables gameVariables;

        [Signal]
        public delegate void ActionPressed();

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            this.GetNode<Label>("ActionName").Text = this.ActionName;
            this.GetNode<Label>("CostBox/CostValue").Text = this.Cost.ToString();
        }

        private void _on_Button_pressed()
        {
            this.gameVariables.Budget -= this.Cost;
            this.EmitSignal(nameof(ActionPressed), this);
        }
    }
}