using Godot;

namespace Menus
{
    public class Pause : Control
    {
        [Signal]
        public delegate void RestartButtonPressed();

        [Signal]
        public delegate void ResumeButtonPressed();

        [Signal]
        public delegate void ScenarioSelectionButtonPressed();

        public override void _Ready()
        {
            this.GetNode<Button>("VBoxContainer/ResumeButton").GrabFocus();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (Input.IsActionJustPressed("ui_cancel"))
            {
                this._on_ResumeButton_pressed();
            }
        }

        private void _on_ResumeButton_pressed()
        {
            this.QueueFree();
            this.EmitSignal(nameof(ResumeButtonPressed));
        }

        private void _on_RestartButton_pressed()
        {
            // We need to see how we can restart a scenario
            this.QueueFree();
            this.EmitSignal(nameof(RestartButtonPressed));
        }

        private void _on_BackToScenarioSelectionButton_pressed()
        {
            // Instead of QueueFree and letting the caller handle this, we should
            // instead directly load the scenario selection scene
            this.QueueFree();
            this.EmitSignal(nameof(ScenarioSelectionButtonPressed));
        }

        private void _on_QuitButton_pressed()
        {
            this.GetTree().Quit();
        }
    }
}