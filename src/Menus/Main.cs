using Godot;

namespace Soteria.Menus
{
    public class Main : Control
    {
        public override void _Ready()
        {
            this.GetTree().Root.Connect("size_changed", this, nameof(this.Resize));

            this.GetNode<Button>("VBoxContainer/StartButton").GrabFocus();

            var gameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            gameVariables.Budget = 1000000000;
        }

        private void _on_StartButton_pressed()
        {
            this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
        }

        private void _on_OptionsButton_pressed()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Hide();

            var optionsScene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
            var optionsSceneInstance = (CanvasLayer)optionsScene.Instance();

            this.GetTree().CurrentScene.AddChild(optionsSceneInstance);

            optionsSceneInstance.Connect(nameof(Options.BackButtonPressed), this, nameof(this._on_Options_BackButton_Signal));
        }
        private void _on_CreditsButton_pressed()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Hide();
            this.GetNode<Node2D>("NetworkGraphRoot").Hide();

            var creditsScene = (PackedScene)ResourceLoader.Load("res://Menus/Credits.tscn");
            var creditsSceneInstance = (Node2D)creditsScene.Instance();

            this.GetTree().CurrentScene.AddChild(creditsSceneInstance);

            creditsSceneInstance.Connect(nameof(Credits.CreditsFinished), this, nameof(this._on_Credits_Finished));
        }
        private void _on_Credits_Finished()
        {
            this.GetNode<VBoxContainer>("VBoxContainer").Show();
            this.GetNode<Node2D>("NetworkGraphRoot").Show();
            this.GetNode<Button>("VBoxContainer/CreditsButton").GrabFocus();
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
