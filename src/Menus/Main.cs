using Godot;

namespace Soteria.Menus
{
    public class Main : Control
    {
        public override void _Ready()
        {
            this.GetTree().Root.Connect("size_changed", this, nameof(this.Resize));

            this.GetNode<Button>("VBoxContainer/StartButton").GrabFocus();
        }

        private void _on_StartButton_pressed()
        {
            this.GetTree().ChangeScene("res://Scenarios/Scenario1/Scenario1.tscn");
        }

        private void _on_OptionsButton_pressed()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Hide();

            var optionsScene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
            var optionsSceneInstance = (Control)optionsScene.Instance();

            this.GetTree().CurrentScene.AddChild(optionsSceneInstance);

            optionsSceneInstance.Connect("BackButtonPressed", this, "_on_Options_BackButton_Signal");
        }

        private void Resize()
        {
            var visibleRect = this.GetTree().Root.GetVisibleRect();
            var background = this.GetNode<TextureRect>("TextureRect");
            background.RectPosition = visibleRect.Position;
            background.RectSize = visibleRect.Size;
        }

        private void _on_Options_BackButton_Signal()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Show();
            this.GetNode<Button>("VBoxContainer/OptionsButton").GrabFocus();
        }

        private void _on_QuitButton_pressed()
        {
            this.GetTree().Quit();
        }
    }
}