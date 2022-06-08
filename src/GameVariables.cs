using Godot;

namespace Soteria
{
    public class GameVariables : Node
    {
        public float WorkSatisfaction = 1.0f;
        public float CustomerSatisfaction = 1.0f;

        public int NodeAffectedByDenialOfService = 0;

        public int AttemptedInfections = 0;
        public int SuccessfulInfections = 0;

        private int budget;
        private int upkeep;
        private int income;

        private int day;
        private int week;
        private Timer dailyTimer;

        [Signal]
        public delegate void BudgetChanged(int budget);

        [Signal]
        public delegate void DateIncreasedDay();

        [Signal]
        public delegate void IncomeChanged(int income);

        [Signal]
        public delegate void TimeScaleChanged(int timescale);

        [Signal]
        public delegate void UpkeepChanged(int upkeep);

        [Signal]
        public delegate void WeekChanged(int week);

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

        public int Income
        {
            get
            {
                return this.income;
            }
            set
            {
                this.income = value;
                this.EmitSignal(nameof(IncomeChanged), this.income);
            }
        }

        public override void _Ready()
        {
            this.dailyTimer = new Timer();
            this.dailyTimer.OneShot = false;
            this.dailyTimer.PauseMode = PauseModeEnum.Stop;
            this.dailyTimer.WaitTime = 1.0f;
            this.dailyTimer.Connect("timeout", this, nameof(this.Dailytimer_callback));
            this.dailyTimer.Autostart = true;
            this.AddChild(this.dailyTimer);
        }

        public void SetTimeScale(int timeScale)
        {
            switch (timeScale)
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

            this.EmitSignal(nameof(TimeScaleChanged), timeScale);
        }

        private void Dailytimer_callback()
        {
            this.Budget += (int)(this.Income * this.GetModifiedCustomerSatisfaction() - this.Upkeep) / 7;

            this.day = (this.day + 1) % 7;
            if (this.day == 6)
            {
                this.Weeklytimer_callback();
            }

            this.EmitSignal(nameof(DateIncreasedDay));
        }

        private void Weeklytimer_callback()
        {
            this.week += 1;
            this.EmitSignal(nameof(WeekChanged), this.week);
        }

        private float GetModifiedCustomerSatisfaction()
        {
            var denialOfServiceFactor = 1 - Mathf.Max(this.NodeAffectedByDenialOfService * 0.1f, 0.4f);

            return this.CustomerSatisfaction * denialOfServiceFactor;
        }
    }
}