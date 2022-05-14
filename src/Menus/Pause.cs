using Godot;

namespace Soteria.Menus
{
    public class Pause : Control
    {
        private bool isPaused;

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
        }

        private void _setIsPaused(bool value)
        {
            this.isPaused = value;
            this.GetTree().Paused = this.IsPaused;
            this.Visible = this.IsPaused;

            if (this.IsPaused)
            {
                this.GetNode<Button>("VBoxContainer/ResumeButton").GrabFocus();
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
            this.GetTree().ChangeScene("res://Menus/ScenarioSelect.tscn");
        }

        private void _on_QuitButton_pressed()
        {
            this.GetTree().Quit();
        }
    }
}