using Godot;

namespace Soteria
{
    public class GameVariables : Node
    {
        public float WorkSatisfaction = 1.0f;
        public float CustomerSatisfaction = 1.0f;

        public int CostToUpgrade;

        public int AttemptedInfections;
        public int SuccessfulInfections;

        public float BackupRestoreSuccessful = 1.0f;

        private int budget;
        private int upkeep;
        private int baseIncome;
        private int currentInfections;

        private int day;
        private int week;
        private Timer dailyTimer;
        private int nodesAffectedByDenialOfService;

        private Audio audio;

        [Signal]
        public delegate void BudgetChanged(int budget);

        [Signal]
        public delegate void CurrentInfectionsChanged(int currentInfections);

        [Signal]
        public delegate void DateIncreasedDay();

        [Signal]
        public delegate void IncomeChanged(int income);

        [Signal]
        public delegate void NoMoreMoney();

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

        public int BaseIncome
        {
            get
            {
                return this.baseIncome;
            }
            set
            {
                this.baseIncome = value;
                this.EmitSignal(nameof(IncomeChanged), this.ActualIncome);
            }
        }

        public int ActualIncome
        {
            get
            {
                return (int)(this.baseIncome * this.GetModifiedCustomerSatisfaction());
            }
        }

        public int CurrentInfections
        {
            get
            {
                return this.currentInfections;
            }
            set
            {
                this.currentInfections = value;
                this.audio.IsInfected = this.currentInfections > 0 ? 1f : 0f;
                this.EmitSignal(nameof(CurrentInfectionsChanged), this.currentInfections);
            }
        }

        public int NodesAffectedByDenialOfService
        {
            get
            {
                return this.nodesAffectedByDenialOfService;
            }
            set
            {
                if (this.nodesAffectedByDenialOfService == value)
                {
                    return;
                }

                this.nodesAffectedByDenialOfService = value;
                this.EmitSignal(nameof(IncomeChanged), this.ActualIncome);
            }
        }

        public string CurrentScenarioName { get; set; }

        public override void _Ready()
        {
            this.dailyTimer = new Timer();
            this.dailyTimer.OneShot = false;
            this.dailyTimer.PauseMode = PauseModeEnum.Stop;
            this.dailyTimer.WaitTime = 1.0f;
            this.dailyTimer.Connect("timeout", this, nameof(this.Dailytimer_callback));
            this.dailyTimer.Autostart = true;
            this.AddChild(this.dailyTimer);

            this.audio = this.GetNode<Audio>("/root/Audio");
        }

        public void ResetValues()
        {
            this.WorkSatisfaction = 1.0f;
            this.CustomerSatisfaction = 1.0f;

            this.CostToUpgrade = 0;

            this.AttemptedInfections = 0;
            this.SuccessfulInfections = 0;

            this.BackupRestoreSuccessful = 1.0f;

            this.Budget = 0;
            this.Upkeep = 0;
            this.BaseIncome = 0;
            this.CurrentInfections = 0;
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
            this.Budget += (this.ActualIncome - this.Upkeep) / 7;

            if (this.Budget <= 0)
            {
                this.Budget = 0;

                this.SetTimeScale(0);
                this.EmitSignal(nameof(NoMoreMoney));
                return;
            }

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
            var denialOfServiceFactor = 1 - Mathf.Min(this.NodesAffectedByDenialOfService * 0.1f, 0.4f);

            return this.CustomerSatisfaction * denialOfServiceFactor;
        }
    }
}
