using Godot;

using Soteria.Menus;
using Soteria.UI.MetaActions;

namespace Soteria.UI
{
    public class MetaActionsScene : Control
    {
        private GameVariables gameVariables;

        private int backupBudgetMax;
        private int currentBackupBudget;
        private MetaActionSlider backupSlider;

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            var trainEmployeesButton = this.GetNode<MetaActionButton>("VBoxContainer/TrainEmployees");
            trainEmployeesButton.Connect(nameof(MetaActionButton.InfoButtonPressed), this, nameof(this.OnInfoButtonTrainEmployees));
            var tfaSwitch = this.GetNode<MetaActionSwitch>("VBoxContainer/2FA");
            tfaSwitch.Connect(nameof(MetaActionSwitch.InfoButtonPressed), this, nameof(this.OnInfoButtonTfa));

            this.backupSlider = this.GetNode<MetaActionSlider>("VBoxContainer/BackupBudget");
            this.backupSlider.Connect(nameof(MetaActionSlider.InfoButtonPressed), this, nameof(this.OnInfoButtonBackupBudget));
            this.backupBudgetMax = this.backupSlider.MaxValue;
            this.backupSlider.Connect(nameof(MetaActionSlider.SliderValueChanged), this, nameof(this.OnBackupBudgetChanged));
            this.gameVariables.Connect(nameof(GameVariables.DateIncreasedDay), this, nameof(this.OnDateIncrease));
            this.OnBackupBudgetChanged(this.backupSlider.DefaultValue);
            this.OnDateIncrease();
        }

        private void OnBackupBudgetChanged(int value)
        {
            this.currentBackupBudget = value;
        }

        private void OnDateIncrease()
        {
            // Currently this is linear, maybe another formular has better values
            // Also it does not allow for 100% change, but only 99.99%
            var backupSuccessfulChance = this.currentBackupBudget / (this.backupBudgetMax + 1f);
            GD.Print($"Chance of successful restore: {backupSuccessfulChance * 100}%");
            this.gameVariables.BackupRestoreSuccessful = backupSuccessfulChance;
        }

        private void OnInfoButtonTrainEmployees()
        {
            var infoBoxLoader = (PackedScene)ResourceLoader.Load("res://Menus/InfoScene.tscn");
            var infoBox = (InfoScene)infoBoxLoader.Instance();
            infoBox.Title = "Training Employees";
            var description = "";
            description += "Humans are the weakest part of your information security system - Michael Brophy\n";
            description += "\n";
            description += "TODO Add Description\n";
            description += "TODO Add references\n";
            infoBox.Description = description;
            this.GetTree().CurrentScene.AddChild(infoBox);
        }

        private void OnInfoButtonBackupBudget()
        {
            var infoBoxLoader = (PackedScene)ResourceLoader.Load("res://Menus/InfoScene.tscn");
            var infoBox = (InfoScene)infoBoxLoader.Instance();
            infoBox.Title = "Backups";
            var description = "";
            description += "If anything goes wrong you still are able to go back to a previously known good version.\n";
            description += "\n";
            description += "TODO Add Description\n";
            description += "TODO Add references\n";
            infoBox.Description = description;
            this.GetTree().CurrentScene.AddChild(infoBox);
        }

        private void OnInfoButtonTfa()
        {
            var infoBoxLoader = (PackedScene)ResourceLoader.Load("res://Menus/InfoScene.tscn");
            var infoBox = (InfoScene)infoBoxLoader.Instance();
            infoBox.Title = "Two Factor Authentication";
            var description = "";
            description += "TODO Add Description\n";
            description += "TODO Add references\n";
            infoBox.Description = description;
            this.GetTree().CurrentScene.AddChild(infoBox);
        }
    }
}