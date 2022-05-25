using Godot;

namespace Soteria
{
	public class GameVariables : Node
	{
		[Signal]
		public delegate void BudgetChanged();

		[Signal]
		public delegate void UpkeepChanged();

		public int Budget
		{
			get => _budget;
			set
			{
				_budget = value;
				this.EmitSignal(nameof(BudgetChanged), _budget);
			}
		}

		public float WorkSatisfaction = 1.0f;

		public int Upkeep
		{
			get => _upkeep;
			set
			{
				_upkeep = value;
				this.EmitSignal(nameof(UpkeepChanged), _upkeep);
			}
		}

		private Timer _weeklyTimer;
		private int _budget = 0;
		private int _upkeep = 0;

		public override void _Ready()
		{
			this._weeklyTimer = new Timer();
			this._weeklyTimer.OneShot = false;
			this._weeklyTimer.PauseMode = PauseModeEnum.Stop;
			this._weeklyTimer.WaitTime = 5.0f;
			this._weeklyTimer.Connect("timeout", this, nameof(_weeklytimer_callback));
			this._weeklyTimer.Autostart = true;
			this.AddChild(this._weeklyTimer);
		}

		private void _weeklytimer_callback()
		{
			this.Budget -= this.Upkeep;
			GD.Print("Budget: " + this.Budget.ToString());
			GD.Print("Weekly upkeep: " + this.Upkeep.ToString());
		}
	}
}
