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
    }
}