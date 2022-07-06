using Godot;

namespace Soteria.UI.MetaActions
{
    public class MetaActionSlider : VBoxContainer
    {
        [Export]
        public string ActionName;

        [Export]
        public int MinValue;

        [Export]
        public int MaxValue;

        [Export]
        public int Step;

        [Export]
        public int DefaultValue;

        private GameVariables gameVariables;

        private int oldValue;

        [Signal]
        public delegate void InfoButtonPressed();

        [Signal]
        public delegate void SliderValueChanged(int value);

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            this.GetNode<Label>("Headline/ActionName").Text = this.ActionName;
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = this.DefaultValue.ToString();

            var slider = this.GetNode<HSlider>("Slider");
            slider.MinValue = this.MinValue;
            slider.MaxValue = this.MaxValue;
            slider.Step = this.Step;
            slider.Value = this.DefaultValue;

            var infoButton = this.GetNode<Button>("Headline/InfoButton");
            infoButton.Connect("pressed", this, nameof(this.OnInfoButtonPressed));
        }

        private void _on_Slider_value_changed(float value)
        {
            var valueInt = (int)value;
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = valueInt.ToString();
            this.gameVariables.Upkeep += valueInt;
            this.gameVariables.Upkeep -= this.oldValue;
            this.oldValue = valueInt;
            this.EmitSignal(nameof(SliderValueChanged), valueInt);
        }

        private void OnInfoButtonPressed()
        {
            this.EmitSignal(nameof(InfoButtonPressed));
        }
    }
}