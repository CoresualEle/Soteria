using Godot;

namespace Soteria.Menus
{
    public class OptionsGraphics : Control
    {
        [Signal]
        public delegate void BackButtonPressed();

        public override void _Ready()
        {
            this.GetNode<Button>("VBoxContainer/FullScreenButton").GrabFocus();
            this.GetNode<CheckButton>("VBoxContainer/FullScreenButton").Disabled = OS.WindowFullscreen;
        }

        private void _on_FullScreenButton_toggled(bool button_pressed)
        {
            OS.WindowFullscreen = button_pressed;
        }

        private void _on_BackButton_pressed()
        {
            this.QueueFree();
            this.EmitSignal(nameof(BackButtonPressed));
        }
    }
}