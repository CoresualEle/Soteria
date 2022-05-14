using System;
using System.Collections.Generic;

using Soteria.Foundation.Contracts;

namespace Soteria.Foundation
{
    public class Virus : IAggressor, IDisposable
    {
        private readonly INetworkGraph networkGraph;
        private readonly IList<INetworkNode> infectedNodes = new List<INetworkNode>();

        public Virus(INetworkNode originatingNode, INetworkGraph networkGraph)
        {
            this.networkGraph = networkGraph;
            this.networkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;

            this.infectedNodes.Add(originatingNode);
        }

        private void Spread()
        {
            var targets = new List<INetworkNode>(this.infectedNodes);

            foreach (var infectedNode in targets)
            {
                foreach (var connection in infectedNode.Connections)
                {
                    if (!connection.Target.Infections.Contains(this) && connection.Target.Attack(this))
                    {
                        this.infectedNodes.Add(connection.Target);
                    }
                }
            }
        }

        private void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
            this.Spread();
        }

        public void Dispose()
        {
            this.networkGraph.NetworkTick -= this.NetworkGraph_OnNetworkTick;
        }
    }
}