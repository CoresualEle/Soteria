using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components
{
    public class Server : Node2D, INetworkNode
    {
        private readonly Random randomizer = new Random();

        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        private readonly float ChanceToSpawnVirus = 0.0f;

        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        private readonly float ThreatResistance = 0.8f;

        private INetworkGraph networkGraph;

        public IList<INetworkConnection> Connections { get; } = new List<INetworkConnection>();

        public IList<IThreat> Infections { get; private set; }

        public override void _Ready()
        {
            this.Infections = new List<IThreat>();

            this.networkGraph = this.GetNode<INetworkGraph>(new NodePath(".."));
            this.networkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;

            this.GetNode<Label>("CanvasLayer/ContextMenu/VBoxContainer/Label").Text = this.Name;
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
                }
            }
        }

        private void _on_Area2D_input_event(object viewport, object @event, int shape_idx)
        {
            if (@event is InputEventMouseButton && ((InputEventMouseButton)@event).ButtonIndex == (int)ButtonList.Left)
            {
                var contextMenu = this.GetNode<PopupPanel>("CanvasLayer/ContextMenu");
                contextMenu.Popup_();

                contextMenu.MarginLeft = this.Position.x;
                contextMenu.MarginTop = this.Position.y;
            }
        }
    }
}