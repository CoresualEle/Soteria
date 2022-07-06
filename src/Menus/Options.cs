using Godot;

namespace Soteria.Menus
{
    public class Options : CanvasLayer
    {
        [Signal]
        public delegate void BackButtonPressed();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            this.GetNode<Button>("VBoxContainer/GraphicsButton").GrabFocus();
        }

        private void _on_GraphicsButton_pressed()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Hide();

            var optionsGraphicsScene = (PackedScene)ResourceLoader.Load("res://Menus/OptionsGraphics.tscn");
            var optionsGraphicsSceneInstance = (CanvasLayer)optionsGraphicsScene.Instance();

            this.GetTree().CurrentScene.AddChild(optionsGraphicsSceneInstance);

            optionsGraphicsSceneInstance.Connect(nameof(OptionsGraphics.BackButtonPressed), this, nameof(this._on_Graphics_BackButton_Signal));
        }

        private void _on_Graphics_BackButton_Signal()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Show();
            this.GetNode<Button>("VBoxContainer/GraphicsButton").GrabFocus();
        }

        private void _on_AudioButton_pressed()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Hide();

            var optionsAudioScene = (PackedScene)ResourceLoader.Load("res://Menus/OptionsAudio.tscn");
            var optionsAudioSceneInstance = (CanvasLayer)optionsAudioScene.Instance();

            this.GetTree().CurrentScene.AddChild(optionsAudioSceneInstance);

            optionsAudioSceneInstance.Connect(nameof(OptionsAudio.BackButtonPressed), this, nameof(this._on_Audio_BackButton_Signal));
        }

        private void _on_Audio_BackButton_Signal()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Show();
            this.GetNode<Button>("VBoxContainer/AudioButton").GrabFocus();
        }

        private void _on_BackButton_pressed()
        {
            this.QueueFree();
            this.EmitSignal(nameof(BackButtonPressed));
        }
    }
}