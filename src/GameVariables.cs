using Godot;

namespace Soteria
{
    public class GameVariables : Node
    {
        public int Budget = 0;
        public float WorkSatisfaction = 1.0f;
        public int Upkeep = 0;

        private Timer weeklyTimer;

        public override void _Ready()
        {
            this.weeklyTimer = new Timer();
            this.weeklyTimer.OneShot = false;
            this.weeklyTimer.PauseMode = PauseModeEnum.Stop;
            this.weeklyTimer.WaitTime = 5.0f;
            this.weeklyTimer.Connect("timeout", this, "_weeklytimer_callback");
            this.weeklyTimer.Autostart = true;
            this.AddChild(this.weeklyTimer);
        }

        private void _weeklytimer_callback()
        {
            this.Budget -= this.Upkeep;
            GD.Print("Budget: " + this.Budget.ToString());
            GD.Print("Weekly upkeep: " + this.Upkeep.ToString());
        }
    }
}