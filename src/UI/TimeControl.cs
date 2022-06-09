using Godot;
using System;

namespace Soteria.UI
{
    public class TimeControl : Control
    {
        private readonly Color colorActive = new Color("ffffff");
        private readonly Color colorNormal = new Color("aaaaaa");
        private GameVariables gameVariables;

        private DateTime currentTime;
        private Label currentTimeLabel;
        private Label currentWeekLabel;
        private Button pauseNode;
        private Button speed1XNode;
        private Button speed2XNode;

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            this.currentTime = DateTime.Now;
            this.currentTimeLabel = this.GetNode<Label>("VBoxContainer/CurrentTimeContainer/CurrentTime");
            this.currentTimeLabel.Text = this.currentTime.ToString("yyyy-MM-dd");

            this.currentWeekLabel = this.GetNode<Label>("VBoxContainer/CurrentTimeContainer/WeekValue");

            this.pauseNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Pause");
            this.speed1XNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Speed1X");
            this.speed2XNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Speed2X");
            this.SetActive(this.speed1XNode);

            this.gameVariables.Connect(nameof(GameVariables.DateIncreasedDay), this, nameof(this._on_DateIncreased));
            this.gameVariables.Connect(nameof(GameVariables.WeekChanged), this, nameof(this._on_WeekChanged));
            this.gameVariables.Connect(nameof(GameVariables.TimeScaleChanged), this, nameof(this.OnTimeScaleChanged));
        }

        private void _on_DateIncreased()
        {
            this.currentTime = this.currentTime.AddDays(1);
            this.currentTimeLabel.Text = this.currentTime.ToString("yyyy-MM-dd");
        }

        private void _on_WeekChanged(int week)
        {
            this.currentWeekLabel.Text = week.ToString();
        }

        private void _on_Pause_pressed()
        {
            this.SetActive(this.pauseNode);
            this.gameVariables.SetTimeScale(0);
        }

        private void _on_Speed1X_pressed()
        {
            this.SetActive(this.speed1XNode);
            this.gameVariables.SetTimeScale(1);
        }

        private void _on_Speed2X_pressed()
        {
            this.SetActive(this.speed2XNode);
            this.gameVariables.SetTimeScale(2);
        }

        private void SetActive(Button node)
        {
            this.pauseNode.Modulate = this.colorNormal;
            this.speed1XNode.Modulate = this.colorNormal;
            this.speed2XNode.Modulate = this.colorNormal;

            node.Modulate = this.colorActive;
        }

        private void OnTimeScaleChanged(int timescale)
        {
            switch(timescale)
            {
                case 0:
                    this.SetActive(this.pauseNode);
                    break;
                case 1:
                    this.SetActive(this.speed1XNode);
                    break;
                case 2:
                    this.SetActive(this.speed2XNode);
                    break;
            }
        }
    }
}