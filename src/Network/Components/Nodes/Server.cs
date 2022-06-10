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
        private float softwareFirewallEffectiveness = 1f;

        [Export]
        private bool policyAntivirus;
        private float antivirusEffectiveness = 2f;

        private Label currentInfectionsLabel;

        public override void _Ready()
        {
            base._Ready();

            this.BaseThreatResistance = 0.5f;

            this.GetNode<Polygon2D>("Polygon2D").Color = this.NormalColor;

            this.SetupMenu();
            this.UpdateThreatResistance();
        }

        public override bool AttemptInfection(IThreat threat)
        {
            this.GameVariables.AttemptedInfections += 1;
            if (!this.Infections.Contains(threat) && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                this.GameVariables.SuccessfulInfections += 1;
                this.Infections.Add(threat);
                this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;
                var currentInfections = "Current Infections: ";
                foreach(var infection in this.Infections) {
                    currentInfections += $"{infection.Name}, ";
                }
                this.currentInfectionsLabel.Text = currentInfections.TrimEnd(", ".ToCharArray());

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

            this.currentInfectionsLabel = this.GetNode<Label>("CanvasLayer/ContextMenu/VBoxContainer/InfectionsLabel");
            this.currentInfectionsLabel.Text = "Current Infections: /";
        }

        private void _on_softwareFirewall_toggled(bool value)
        {
            this.policySoftwareFirewall = value;
            this.UpdateThreatResistance();
        }

        private void _on_antivirus_toggled(bool value)
        {
            this.policyAntivirus = value;
            this.UpdateThreatResistance();
        }

        private void UpdateThreatResistance()
        {
            var softwareFirewallEnabled = this.policySoftwareFirewall ? 1 : 0;
            var adjustmentFactorSoftwareFirewall = this.softwareFirewallEffectiveness * softwareFirewallEnabled;

            var antiVirusEnabled = this.policyAntivirus ? 1 : 0;
            var adjustmentFactorAntiVirus = this.antivirusEffectiveness * antiVirusEnabled;

            var threatResistanceUnadjusted = this.BaseThreatResistance + adjustmentFactorSoftwareFirewall + adjustmentFactorAntiVirus;
            // Use Gaussian Error Function to get diminishing returns on more security
            // Aproximation from Abramowitz and Stegun
            // https://digital.library.unt.edu/ark:/67531/metadc40301/m2/1/high_res_d/applmathser55_w.pdf
            // page 299, formula 7.1.27
            var x = threatResistanceUnadjusted;
            var a1 = 0.278393f;
            var a2 = 0.230389f;
            var a3 = 0.000972f;
            var a4 = 0.078108f;
            this.ThreatResistance = 1f - (1f / Mathf.Pow(1f + a1*x + a2*x*x + a3*x*x*x + a4*x*x*x*x, 4f));

            GD.Print($"ThreatResistance for {this.Name}: {this.ThreatResistance*100}%");
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
            this.currentInfectionsLabel.Text = "Current Infections: /";
            this.GetNode<Polygon2D>("Polygon2D").Color = this.NormalColor;
        }
    }
}