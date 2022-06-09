using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;
using Soteria.UI;

namespace Soteria.Network.Components.Nodes
{
    public class Server : NetworkNodeBase
    {
        [Export]
        private bool policySoftwareFirewall;

        [Export]
        private bool policyAntivirus;

        public override void _Ready()
        {
            base._Ready();

            this.GetNode<Polygon2D>("Polygon2D").Color = this.NormalColor;

            this.SetupMenu();
        }

        public override bool AttemptInfection(IThreat threat)
        {
            this.GameVariables.AttemptedInfections += 1;
            if (!this.Infections.Contains(threat) && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                this.GameVariables.SuccessfulInfections += 1;
                this.Infections.Add(threat);
                this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;

                return true;
            }

            return false;
        }

        private void SetupMenu()
        {
            var softwareFirewallNode = this.GetNode<ContextMenuBoolean>("CanvasLayer/ContextMenu/VBoxContainer/SoftwareFirewall");
            softwareFirewallNode.Connect(nameof(ContextMenuBoolean.ButtonToggled), this, nameof(this._on_softwareFirewall_toggled));
            softwareFirewallNode.Enabled = this.policySoftwareFirewall;

            // We need to explicitely call the _Ready method of the ContextMenuBoolean nodes to get it to be enabled by default
            softwareFirewallNode._Ready();

            var antivirusNode = this.GetNode<ContextMenuBoolean>("CanvasLayer/ContextMenu/VBoxContainer/AntiVirus");
            antivirusNode.Connect(nameof(ContextMenuBoolean.ButtonToggled), this, nameof(this._on_antivirus_toggled));
            antivirusNode.Enabled = this.policyAntivirus;
            antivirusNode._Ready();

            var backupRestoreNode = this.GetNode<ContextMenuAction>("CanvasLayer/ContextMenu/VBoxContainer/BackupRestore");
            backupRestoreNode.Connect(nameof(ContextMenuAction.ActionPressed), this, nameof(this._on_backup_restored));
        }

        private void _on_softwareFirewall_toggled(bool value)
        {
            this.policySoftwareFirewall = value;
        }

        private void _on_antivirus_toggled(bool value)
        {
            this.policyAntivirus = value;
        }

        private void _on_backup_restored()
        {
            if (this.Randomizer.NextDouble() > this.GameVariables.BackupRestoreSuccessful)
            {
                // Backup failed
                // TODO maybe we should also show the user that the backup failed
                return;
            }
            foreach (var threat in this.Infections)
            {
                threat.RemoveNode(this);
            }

            this.Infections.Clear();
            this.GetNode<Polygon2D>("Polygon2D").Color = this.NormalColor;
        }
    }
}