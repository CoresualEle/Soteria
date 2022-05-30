using System;
using System.Collections.Generic;

using Soteria.Foundation.Contracts;

namespace Soteria.Foundation
{
    public class SpreadingThreatBase : IThreat, IDisposable
    {
        private readonly INetworkGraph networkGraph;
        private readonly IList<INetworkNode> infectedNodes = new List<INetworkNode>();

        public SpreadingThreatBase(INetworkNode sourceNode, INetworkGraph networkGraph)
        {
            this.infectedNodes.Add(sourceNode);

            this.networkGraph = networkGraph;
            this.networkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;
        }

        private void Spread()
        {
            var targets = new List<INetworkNode>(this.infectedNodes);

            foreach (var infectedNode in targets)
            {
                foreach (var connection in infectedNode.Connections)
                {
                    if (!connection.Target.Infections.Contains(this) && connection.Target.AttemptInfection(this))
                    {
                        this.infectedNodes.Add(connection.Target);
                    }
                }
            }
        }

        public void RemoveNode(INetworkNode node)
        {
            this.infectedNodes.Remove(node);
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