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
            }

            if (!this.nodes.Contains(connectionToRegister.Target))
            {
                this.nodes.Add(connectionToRegister.Target);
            }
        }

        /// <summary>
        /// Finds the shortest path between two <see cref="INetworkNode" />--> on the graph using Dijkstra's algorithm.
        /// See: https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm#Pseudocode
        /// </summary>
        /// <param name="source">Starting point of the path</param>
        /// <param name="target">Endpoint of the path</param>
        /// <returns>A list of the connections necessary to traverse the graph on the shortest path from source to target</returns>
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

            var distanceIndex = new Dictionary<INetworkNode, int>();
            var previousNodeIndex = new Dictionary<INetworkNode, INetworkNode>();
            var unvisitedNodes = new List<INetworkNode>();

            // Initialize basics
            foreach (var v in this.nodes)
            {
                distanceIndex.Add(v, int.MaxValue);
                previousNodeIndex.Add(v, null);
                unvisitedNodes.Add(v);
            }

            // Starting point has distance of 0
            distanceIndex[source] = 0;

            // Repeat until all nodes are visited or target is found.
            while (unvisitedNodes.Count > 0)
            {
                // Head of the shortest path so far
                var currentPathHead = FindNodeWithMinDistance();

                // Finish loop if target is found
                if (currentPathHead == target)
                {
                    break;
                }

                unvisitedNodes.Remove(currentPathHead);

                // Repeat for each unvisited connected node
                foreach (var connection in currentPathHead.Connections.Where(x => unvisitedNodes.Contains(x.Target)))
                {
                    var sourceToTargetDistance = distanceIndex[currentPathHead] + connection.Weight;

                    // Abort and continue with next connection if current path's distance is longer than another path to the intermediate target
                    if (sourceToTargetDistance >= distanceIndex[connection.Target])
                    {
                        continue;
                    }

                    // Set shorter value as new distance for this node
                    distanceIndex[connection.Target] = sourceToTargetDistance;

                    // Set current node as part of the final path
                    previousNodeIndex[connection.Target] = currentPathHead;
                }
            }

            // If target node has no previous node, no valid path has been found
            if (previousNodeIndex[target] == null)
            {
                throw new ArgumentOutOfRangeException(nameof(target), "No path is available.");
            }

            var path = new List<INetworkConnection>();
            var nextItemToAdd = target;

            // Build the list of connections from back to front
            while (previousNodeIndex[nextItemToAdd] != null)
            {
                var item = this.edges.Single(x => x.Target == nextItemToAdd && x.Source == previousNodeIndex[nextItemToAdd]);
                path.Insert(0, item);
                nextItemToAdd = previousNodeIndex[nextItemToAdd];
            }

            return path;

            // Iterates the list of unvisited nodes and finds the one with the lowest overall path length
            INetworkNode FindNodeWithMinDistance()
            {
                var result = unvisitedNodes.First();

                foreach (var node in unvisitedNodes)
                {
                    if (distanceIndex[node] < distanceIndex[result])
                    {
                        result = node;
                    }
                }

                return result;
            }
        }
    }
}