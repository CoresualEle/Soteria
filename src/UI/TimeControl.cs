using Godot;
using System;
using Soteria;

public class TimeControl : Control
{
    private GameVariables gameVariables;

    private DateTime currentTime;
    private Label currentTimeLabel;
    private Label currentWeekLabel;
    private Button pauseNode;
    private Button speed1xNode;
    private Button speed2xNode;
    
    private Color colorActive = new Color("ffffff");
    private Color colorNormal = new Color("0a0a0a");

    public override void _Ready()
    {
        this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

        this.currentTime = DateTime.Now;
        this.currentTimeLabel = this.GetNode<Label>("VBoxContainer/CurrentTimeContainer/CurrentTime");
        this.currentTimeLabel.Text = this.currentTime.ToString("yyyy-MM-dd");

        
        this.currentWeekLabel = this.GetNode<Label>("VBoxContainer/CurrentTimeContainer/WeekValue");
        
        this.pauseNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Pause");
        this.speed1xNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Speed1X");
        this.speed2xNode = this.GetNode<Button>("VBoxContainer/TimeControlButtons/Speed2X");
        this.setActive(this.speed1xNode);
        
        gameVariables.Connect(nameof(GameVariables.DateIncreasedDay), this, nameof(this._on_DateIncreased));
        gameVariables.Connect(nameof(GameVariables.WeekChanged), this, nameof(this._on_WeekChanged));
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
        this.setActive(this.pauseNode);
        this.gameVariables.setTimeScale(0);
    }
    private void _on_Speed1X_pressed()
    {
        this.setActive(this.speed1xNode);
        this.gameVariables.setTimeScale(1);
    }
    private void _on_Speed2X_pressed()
    {
        this.setActive(this.speed2xNode);
        this.gameVariables.setTimeScale(2);
    }
    private void setActive(Button node)
    {
        this.pauseNode.Modulate = colorNormal;
        this.speed1xNode.Modulate = colorNormal;
        this.speed2xNode.Modulate = colorNormal;

        node.Modulate = colorActive;

    }
}
