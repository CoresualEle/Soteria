using Godot;
using Soteria.Menus;

namespace Soteria.Scenarios.Scenario1
{
    public class Scenario1 : Node2D
    {

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