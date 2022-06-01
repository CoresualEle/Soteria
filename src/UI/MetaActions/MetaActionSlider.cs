using Godot;
using Soteria;
using System;

namespace Soteria.UI.MetaActions {
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

        [Signal]
        public delegate void SliderValueChanged(int value);

        private GameVariables gameVariables;

        private int oldValue = 0;

        public override void _Ready()
        {
            this.gameVariables = GetNode<GameVariables>("/root/GameVariables");
            this.GetNode<Label>("ActionName").Text = this.ActionName;
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = this.DefaultValue.ToString();

            var slider = this.GetNode<HSlider>("Slider");
            slider.MinValue = this.MinValue;
            slider.MaxValue = this.MaxValue;
            slider.Step = this.Step;
            slider.Value = this.DefaultValue;
        }

        private void _on_Slider_value_changed(float value)
        {
            GD.Print("Called function");
            var valueInt = (int) value;
            this.GetNode<Label>("UpkeepBox/UpkeepValue").Text = valueInt.ToString();
            this.gameVariables.Upkeep += valueInt;
            this.gameVariables.Upkeep -= oldValue;
            this.oldValue = valueInt;
            this.EmitSignal(nameof(SliderValueChanged), this, valueInt);
        }
    }
}
