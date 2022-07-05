using Godot;

namespace Soteria.Menus
{
    public class OptionsGraphics : CanvasLayer
    {
        [Signal]
        public delegate void BackButtonPressed();

        public override void _Ready()
        {
            this.GetNode<Button>("VBoxContainer/FullScreenButton").GrabFocus();
            this.GetNode<CheckButton>("VBoxContainer/FullScreenButton").Pressed = OS.WindowFullscreen;
        }

        private void _on_FullScreenButton_toggled(bool buttonPressed)
        {
            OS.WindowFullscreen = buttonPressed;
        }

        private void _on_BackButton_pressed()
        {
            this.GetNode<UserPreferences>("/root/UserPreferences").SavePreferences();
            this.QueueFree();
            this.EmitSignal(nameof(BackButtonPressed));
        }
    }
}
