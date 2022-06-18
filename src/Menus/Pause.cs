using Godot;

namespace Soteria.Menus
{
    public class Pause : Control
    {
        private bool isPaused;
        private Audio audio;
        private float previousAudio = 0f;

        [Signal]
        public delegate void RestartButtonPressed();

        // Could be that we will not use the ResumeButtonPressed signal
        [Signal]
        public delegate void ResumeButtonPressed();

        public bool IsPaused
        {
            get
            {
                return this.isPaused;
            }
            set
            {
                this._setIsPaused(value);
            }
        }

        public override void _Ready()
        {
            this.IsPaused = false;
            this.audio = this.GetNode<Audio>("/root/Audio");
        }

        private void _setIsPaused(bool value)
        {
            this.isPaused = value;
            this.GetTree().Paused = this.IsPaused;
            this.Visible = this.IsPaused;

            // Sometimes the value is adjusted, before _Ready was called
            // So we have a quick fix to make sure that this.audio has a valid value
            if (this.audio is null)
            {
                this.audio = this.GetNode<Audio>("/root/Audio");
            }

            if (this.IsPaused)
            {
                this.GetNode<Button>("VBoxContainer/ResumeButton").GrabFocus();
                this.previousAudio = this.audio.IsInfected;
                this.audio.IsInfected = 0f;
                this.audio.IsIngame = 0f;
            } else
            {
                this.audio.IsIngame = 1f;
                this.audio.IsInfected = this.previousAudio;
            }
        }

        public override void _UnhandledInput(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed("ui_cancel"))
            {
                this._on_ResumeButton_pressed();
            }
        }

        private void _on_ResumeButton_pressed()
        {
            this.IsPaused = false;
            this.EmitSignal(nameof(ResumeButtonPressed));
        }

        private void _on_RestartButton_pressed()
        {
            this.IsPaused = false;
            this.EmitSignal(nameof(RestartButtonPressed));
        }

        private void _on_BackToScenarioSelectionButton_pressed()
        {
            this.IsPaused = false;
            this.audio.IsInfected = 0f;
            this.audio.IsIngame = 0f;
            this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
        }

        private void _on_OptionsButton_pressed()
        {
            this.Hide();
            this.audio.IsInfected = 0f;
            this.audio.IsIngame = 0f;
            var optionsScene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
            var optionsSceneInstance = (CanvasLayer)optionsScene.Instance();

            this.GetTree().CurrentScene.AddChild(optionsSceneInstance);

            optionsSceneInstance.Connect(nameof(Options.BackButtonPressed), this, nameof(this._on_Options_BackButton_Signal));
        }

        private void _on_Options_BackButton_Signal()
        {
            this.Show();
            this.GetNode<Button>("VBoxContainer/OptionsButton").GrabFocus();
        }

        private void _on_QuitButton_pressed()
        {
            this.GetTree().Quit();
        }
    }
}
