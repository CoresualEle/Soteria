using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;
using Soteria.UI;

namespace Soteria.Network.Components
{
    public class Server : Node2D, INetworkNode
    {
        private readonly Color normalColor = new Color("003F7A");
        private readonly Color infectedColor = new Color("7D1022");
        private readonly Random randomizer = new Random();

        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        private readonly float ChanceToSpawnVirus = 0.0f;

        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        private readonly float ThreatResistance = 0.8f;

        private INetworkGraph networkGraph;

        [Export]
        private bool policySoftwareFirewall;

        [Export]
        private bool policyAntivirus;

        public IList<INetworkConnection> Connections { get; } = new List<INetworkConnection>();

        public IList<IThreat> Infections { get; private set; }

        public override void _Ready()
        {
            this.Infections = new List<IThreat>();

            this.networkGraph = this.GetNode<INetworkGraph>(new NodePath(".."));
            this.networkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;

            this.GetNode<Label>("CanvasLayer/ContextMenu/VBoxContainer/Label").Text = this.Name;
            this.GetNode<Polygon2D>("Polygon2D").Color = this.normalColor;

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

        public void AddConnection(INetworkConnection connection)
        {
            if (!this.Connections.Contains(connection))
            {
                this.Connections.Add(connection);
            }
        }

        public bool AttemptInfection(IThreat threat)
        {
            if (!this.Infections.Contains(threat) && this.randomizer.NextDouble() > this.ThreatResistance)
            {
                this.Infections.Add(threat);
                this.GetNode<Polygon2D>("Polygon2D").Color = this.infectedColor;

                return true;
            }

            return false;
        }

        private void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
            if (this.ChanceToSpawnVirus > 0 && this.randomizer.NextDouble() <= this.ChanceToSpawnVirus && this.randomizer.NextDouble() > this.ThreatResistance)
            {
                var newThreat = new SpreadingThreatBase(this, this.networkGraph);
                if (!this.Infections.Contains(newThreat))
                {
                    this.Infections.Add(newThreat);
                    this.GetNode<Polygon2D>("Polygon2D").Color = this.infectedColor;
                }
            }
        }

        private void _on_Area2D_input_event(Node viewport, InputEvent @event, int shapeIdx)
        {
            if (@event is InputEventMouseButton && ((InputEventMouseButton)@event).ButtonIndex == (int)ButtonList.Left)
            {
                var contextMenu = this.GetNode<PopupPanel>("CanvasLayer/ContextMenu");
                contextMenu.Popup_();

                contextMenu.MarginLeft = this.Position.x;
                contextMenu.MarginTop = this.Position.y;
            }
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
            this.GetNode<Polygon2D>("Polygon2D").Color = this.normalColor;
        }
    }
}