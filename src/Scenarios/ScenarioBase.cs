using Godot;

using Soteria.Menus;
using Soteria.Network;

namespace Soteria.Scenarios
{
    public class ScenarioBase : Node2D
    {
        protected GameVariables GameVariables;

        public override void _Ready()
        {
            this.GameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            this.GameVariables.AttemptedInfections = 0;
            this.GameVariables.SuccessfulInfections = 0;

            var networkGraphInstance = this.GetNode<NetworkGraph>("NetworkGraphRoot");
            this.GameVariables.Connect(nameof(GameVariables.DateIncreasedDay), networkGraphInstance, "_on_GameTickTimer_timeout");

            RegisterGameOver();

            this.GameVariables.SetTimeScale(1);
        }

        protected virtual void RegisterGameOver()
        {
            this.GameVariables.Connect(nameof(GameVariables.NoMoreMoney), this, nameof(DisplayGameOver));
        }

        private void DisplayGameOver()
        {
            this.GetTree().ChangeScene("Menus/GameOver.tscn");
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (inputEvent.IsActionPressed("ui_cancel"))
            {
                this.GetTree().SetInputAsHandled();
                this.GetNode<Pause>("UI/Pause").IsPaused = true;
            }
        }
    }
}