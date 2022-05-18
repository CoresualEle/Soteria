using Godot;

using Soteria.Menus;

namespace Soteria.Scenarios
{
    public class ScenarioBase : Node2D
    {
        protected GameVariables gameVariables;

        public override void _Ready()
        {
            this.gameVariables = GetNode<GameVariables>("/root/GameVariables");
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