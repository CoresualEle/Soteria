using System;

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

        protected override void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
            if (this.ChanceToSpawnVirus > 0 && this.Randomizer.NextDouble() <= this.ChanceToSpawnVirus && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                var newThreat = new SpreadingThreatBase(this, this.NetworkGraph);
                if (!this.Infections.Contains(newThreat))
                {
                    this.Infections.Add(newThreat);
                    this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;
                }
            }
        }

        public override bool AttemptInfection(IThreat threat)
        {
            if (!this.Infections.Contains(threat) && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
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
            foreach (var threat in this.Infections)
            {
                threat.RemoveNode(this);
            }

            this.Infections.Clear();
            this.GetNode<Polygon2D>("Polygon2D").Color = this.NormalColor;
        }
    }
}