using Godot;

namespace Soteria
{
    public class GameVariables : Node
    {
        public float WorkSatisfaction = 1.0f;

        private int budget;
        private int upkeep;

        [Signal]
        public delegate void BudgetChanged();

        [Signal]
        public delegate void UpkeepChanged();
        
        [Signal]
        public delegate void DateIncreasedDay();

        [Signal]
        public delegate void WeekChanged(int week);
        
        private int day = 0;
        private int week = 0;
        private Timer dailyTimer;

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
            this.dailyTimer = new Timer();
            this.dailyTimer.OneShot = false;
            this.dailyTimer.PauseMode = PauseModeEnum.Stop;
            this.dailyTimer.WaitTime = 1.0f;
            this.dailyTimer.Connect("timeout", this, nameof(this._dailytimer_callback));
            this.dailyTimer.Autostart = true;
            this.AddChild(this.dailyTimer);
        }

        public void setTimeScale(int timeScale)
        {
            switch(timeScale)
            {
                case 0:
                    this.dailyTimer.Paused = true;
                    break;
                case 1:
                    this.dailyTimer.WaitTime = 1.0f;
                    this.dailyTimer.Paused = false;
                    break;
                case 2:
                    this.dailyTimer.WaitTime = 0.5f;
                    this.dailyTimer.Paused = false;
                    break;
            }
        }

        private void _dailytimer_callback()
        {
            this.day = (this.day + 1) % 7;
            if (this.day == 6)
            {
                _weeklytimer_callback();
            }
            this.EmitSignal(nameof(DateIncreasedDay));
        }

        private void _weeklytimer_callback()
        {
            this.week +=1;
            this.Budget -= this.Upkeep;
            this.EmitSignal(nameof(WeekChanged), this.week);
        }
    }
}