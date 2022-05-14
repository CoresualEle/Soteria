using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components
{
    public class Server : Node2D, INetworkNode
    {
        private INetworkGraph networkGraph;

        public IList<INetworkConnection> Connections { get; } = new List<INetworkConnection>();

        public override void _Ready()
        {
            this.GetNode<Label>("CanvasLayer/ContextMenu/VBoxContainer/Label").Text = this.Name;

            this.networkGraph = this.GetNode<INetworkGraph>(new NodePath(".."));
            this.networkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;
        }

        private void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
        }

        public void AddConnection(INetworkConnection connection)
        {
            if (!this.Connections.Contains(connection))
            {
                this.Connections.Add(connection);
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
