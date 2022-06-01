using Godot;

namespace Soteria.UI.MetaActions
{
    public class MetaActionSwitch : VBoxContainer
    {
        [Export]
        public string ActionName;

        [Export]
        public int Cost;

        [Export]
        public int Upkeep;

        private GameVariables gameVariables;

        [Signal]
        public delegate void ButtonToggled(bool value);

        [Export]
        public bool Enabled { get; set; }

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            this.GetNode<Label>("ActionName").Text = this.ActionName;
            this.GetNode<Label>("CostBox/CostValue").Text = this.Cost.ToString();
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = this.Upkeep.ToString();
            this.GetNode<CheckButton>("SwitchButton").Pressed = this.Enabled;
        }

        private void _on_SwitchButton_toggled(bool value)
        {
            this.EmitSignal(nameof(ButtonToggled), this, value);
            if (value)
            {
                this.gameVariables.Budget -= this.Cost;
                this.gameVariables.Upkeep += this.Upkeep;
            }
            else
            {
                this.gameVariables.Upkeep -= this.Upkeep;
            }
        }
    }
}