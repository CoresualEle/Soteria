using Godot;

namespace Soteria.UI
{
    public class ContextMenuBoolean : VBoxContainer
    {
        [Export]
        public string OptionLabel;

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

            this.GetNode<Label>("Name").Text = this.OptionLabel;
            this.GetNode<Label>("CostBox/CostValue").Text = this.Cost.ToString();
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = this.Upkeep.ToString();
            this.GetNode<CheckButton>("CheckButton").Pressed = this.Enabled;
            if (this.Enabled)
            {
                // Add back Budget if enabled by default
                this.gameVariables.Budget += this.Cost;
            }
        }

        public void _on_CheckButton_toggled(bool value)
        {
            this.EmitSignal(nameof(ButtonToggled), value);
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
