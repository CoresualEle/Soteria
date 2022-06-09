using Godot;
using Soteria;
using Soteria.Menus;
using Soteria.UI.MetaActions;

namespace Soteria.UI {
    public class MetaActionsScene : Control
    {
        private GameVariables gameVariables;

        private int backupBudgetMax = 0;
        private int currentBackupBudget = 0;
        private MetaActionSlider backupSlider;
        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            var trainEmployeesButton = this.GetNode<MetaActionButton>("VBoxContainer/TrainEmployees");
            trainEmployeesButton.Connect(nameof(MetaActionButton.InfoButtonPressed), this, nameof(OnInfoButtonTrainEmployees));
            var backupSlider = this.GetNode<MetaActionSlider>("VBoxContainer/BackupBudget");
            backupSlider.Connect(nameof(MetaActionSlider.InfoButtonPressed), this, nameof(OnInfoButtonBackupBudget));
            var TFASwitch = this.GetNode<MetaActionSwitch>("VBoxContainer/2FA");
            TFASwitch.Connect(nameof(MetaActionSwitch.InfoButtonPressed), this, nameof(OnInfoButtonTFA));

            this.backupSlider = this.GetNode<MetaActionSlider>("VBoxContainer/BackupBudget");
            this.backupBudgetMax = this.backupSlider.MaxValue;
            this.backupSlider.Connect(nameof(MetaActionSlider.SliderValueChanged), this, nameof(OnBackupBudgetChanged));
            this.gameVariables.Connect(nameof(GameVariables.DateIncreasedDay), this, nameof(OnDateIncrease));
            OnBackupBudgetChanged(this.backupSlider.DefaultValue);
            OnDateIncrease();
        }

        private void OnBackupBudgetChanged(int value)
        {
            this.currentBackupBudget = value;
        }

        private void OnDateIncrease()
        {
            // Currently this is linear, maybe another formular has better values
            // Also it does not allow for 100% change, but only 99.99%
            var backupSuccessfulChance = (float) this.currentBackupBudget / ((float) this.backupBudgetMax + 1f);
            GD.Print($"Chance of successful restore: {backupSuccessfulChance*100}%");
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
        private void OnInfoButtonTFA()
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
