using Godot;

using Soteria.Menus;
using Soteria.Network;
using Soteria.UI;

namespace Soteria.Scenarios
{
    public class ScenarioBase : Node2D
    {
        protected GameVariables GameVariables;

        [Signal]
        public delegate void ScenarioLoaded();

        public override void _Ready()
        {
            this.GameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            this.GameVariables.ResetValues();

            var networkGraphInstance = this.GetNode<NetworkGraph>("NetworkGraphRoot");
            this.GameVariables.Connect(nameof(GameVariables.DateIncreasedDay), networkGraphInstance, "_on_GameTickTimer_timeout");

            this.RegisterGameOver();

            this.GameVariables.SetTimeScale(1);

            this.Connect(nameof(ScenarioLoaded), this, nameof(this.OnScenarioLoaded));
            this.GetNode<Audio>("/root/Audio").IsIngame = 1f;
        }

        private void OnScenarioLoaded()
        {
            var scenarioUpgrade = this.GetNode<UpgradeScenario>("UI/UpgradeScenario");
            scenarioUpgrade.OnScenarioLoaded();
        }

        protected virtual void RegisterGameOver()
        {
            this.GameVariables.Connect(nameof(GameVariables.NoMoreMoney), this, nameof(this.DisplayGameOver));
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