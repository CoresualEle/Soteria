using Godot;
using Soteria;
using Soteria.UI.MetaActions;

namespace Soteria.UI {
    public class MetaActionsScene : Control
    {
        private GameVariables gameVariables;

        private int backupBudgetMax = 0;
        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            var backupSlider = this.GetNode<MetaActionSlider>("VBoxContainer/BackupBudget");
            this.backupBudgetMax = backupSlider.MaxValue;
            backupSlider.Connect(nameof(MetaActionSlider.SliderValueChanged), this, nameof(OnBackupBudgetChanged));
            OnBackupBudgetChanged(backupSlider.DefaultValue);
        }

        private void OnBackupBudgetChanged(int value)
        {
            // Currently this is linear, maybe another formular has better values
            // Also it does not allow for 100% change, but only 99.99%
            var backupSuccessfulChance = (float) value / ((float) this.backupBudgetMax + 1f);
            GD.Print($"Chance of successful restore: {backupSuccessfulChance*100}%");
            this.gameVariables.BackupRestoreSuccessful = backupSuccessfulChance;
        }
    }
}
