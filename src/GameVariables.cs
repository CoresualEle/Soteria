using Godot;

namespace Soteria
{
    public class GameVariables : Node
    {
        public float WorkSatisfaction = 1.0f;

        private Timer weeklyTimer;
        private int budget;
        private int upkeep;

        [Signal]
        public delegate void BudgetChanged();

        [Signal]
        public delegate void UpkeepChanged();

        public int Budget
        {
            get
            {
                return this.budget;
            }
            set
            {
                this.budget = value;
                this.EmitSignal(nameof(BudgetChanged), this.budget);
            }
        }

        public int Upkeep
        {
            get
            {
                return this.upkeep;
            }
            set
            {
                this.upkeep = value;
                this.EmitSignal(nameof(UpkeepChanged), this.upkeep);
            }
        }

        public override void _Ready()
        {
            this.weeklyTimer = new Timer();
            this.weeklyTimer.OneShot = false;
            this.weeklyTimer.PauseMode = PauseModeEnum.Stop;
            this.weeklyTimer.WaitTime = 5.0f;
            this.weeklyTimer.Connect("timeout", this, nameof(this._weeklytimer_callback));
            this.weeklyTimer.Autostart = true;
            this.AddChild(this.weeklyTimer);
        }

        private void _weeklytimer_callback()
        {
            this.Budget -= this.Upkeep;
        }
    }
}