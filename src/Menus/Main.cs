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

            var options_scene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
            var options_scene_instance = (Control)options_scene.Instance();

            this.GetTree().CurrentScene.AddChild(options_scene_instance);

            options_scene_instance.Connect("BackButtonPressed", this, "_on_Options_BackButton_Signal");
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
