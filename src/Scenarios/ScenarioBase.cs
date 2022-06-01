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

            var NetworkGraphInstance = this.GetNode<NetworkGraph>("NetworkGraphRoot");
            this.GameVariables.Connect(nameof(GameVariables.DateIncreasedDay), NetworkGraphInstance, "_on_GameTickTimer_timeout");
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