using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components
{
    public class Server : Node2D, INetworkNode
    {
        private const float ChanceToSpawnVirus = 0.2f;
        private const float VirusResistance = 0.5f;

        private readonly Random randomizer = new Random();
        private INetworkGraph networkGraph;

        [Export]
        public bool CanSpawnVirus;

        public IList<INetworkConnection> Connections { get; } = new List<INetworkConnection>();

        public IList<IAggressor> Infections { get; private set; }

        public override void _Ready()
        {
            this.Infections = new List<IAggressor>();

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

        public bool Attack(IAggressor aggressor)
        {
            GD.Print($"Server {this.Name} is attacked");

            if (this.randomizer.NextDouble() > VirusResistance)
            {
                this.Infections.Add(aggressor);

                return true;
            }

            return false;
        }

        private void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
            if (this.CanSpawnVirus && this.Infections.Count == 0)
            {
                if (this.randomizer.NextDouble() <= ChanceToSpawnVirus)
                {
                    this.Infections.Add(new Virus(this, this.networkGraph));

                    GD.Print($"Server {this.Name} is infected.");

                    return;
                }

                GD.Print($"Server {this.Name} resited infection.");
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