using System;
using System.Collections.Generic;
using System.Linq;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Network
{
    public class NetworkGraph : Node2D, INetworkGraph
    {
        private readonly IList<INetworkNode> nodes = new List<INetworkNode>();
        private readonly IList<INetworkConnection> edges = new List<INetworkConnection>();

        private bool initialRun;

        public void RegisterConnection(INetworkConnection connectionToRegister)
        {
            if (!this.edges.Contains(connectionToRegister))
            {
                this.edges.Add(connectionToRegister);
                connectionToRegister.Source.AddConnection(connectionToRegister);
            }

            if (!this.nodes.Contains(connectionToRegister.Source))
            {
                this.nodes.Add(connectionToRegister.Source);
                GD.Print($"Added Node {connectionToRegister.Source.Name}.");
            }

            if (!this.nodes.Contains(connectionToRegister.Target))
            {
                this.nodes.Add(connectionToRegister.Target);
                GD.Print($"Added Node {connectionToRegister.Target.Name}.");
            }
        }

        public IList<INetworkConnection> GetPathToNode(INetworkNode source, INetworkNode target)
        {
            if (source == null || !this.nodes.Contains(source))
            {
                throw new ArgumentOutOfRangeException(nameof(source), "Given source is not part of the known Nodes.");
            }

            if (target == null || !this.nodes.Contains(target))
            {
                throw new ArgumentOutOfRangeException(nameof(target), "Given target is not part of the known Nodes.");
            }

            if (source == target)
            {
                throw new ArgumentException("Target is the same instance as source.", nameof(target));
            }

            var vertices = this.nodes;
            var dist = new Dictionary<INetworkNode, int>();
            var prev = new Dictionary<INetworkNode, INetworkNode>();
            var Q = new List<INetworkNode>();

            foreach (var v in vertices)
            {
                dist.Add(v, int.MaxValue);
                prev.Add(v, null);
                Q.Add(v);
            }

            dist[source] = 0;

            while (Q.Count > 0)
            {
                var u = FindNodeWithMinDistance();

                if (u == target)
                {
                    break;
                }

                Q.Remove(u);

                foreach (var connection in u.Connections.Where(x => Q.Contains(x.Target)))
                {
                    var alt = dist[u] + connection.Weight;
                    if (alt < dist[connection.Target])
                    {
                        dist[connection.Target] = alt;
                        prev[connection.Target] = u;
                    }
                }
            }

            if (prev[target] == null)
            {
                throw new ArgumentOutOfRangeException(nameof(target), "No path is available.");
            }

            var path = new List<INetworkConnection>();
            var r = target;

            {
                while (prev[r] != null)
                {
                    var item = this.edges.Single(x => x.Target == r && x.Source == prev[r]);
                    path.Insert(0, item);
                    GD.Print($"Added {item.Source.Name} -> {item.Target.Name}");
                    GD.Print($"prev is {prev[r].Name}");
                    r = prev[r];
                }
            }

            return path;

            INetworkNode FindNodeWithMinDistance()
            {
                var result = Q.First();

                foreach (var node in Q)
                {
                    if (dist[node] < dist[result])
                    {
                        result = node;
                    }
                }

                return result;
            }
        }
    }
}