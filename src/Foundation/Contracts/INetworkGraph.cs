using System.Collections.Generic;

namespace Soteria.Foundation.Contracts
{
    public interface INetworkGraph
    {
        void RegisterConnection(INetworkConnection connectionToRegister);

        /// <summary>
        /// Finds the shortest path between two <see cref="INetworkNode" />--> on the graph using Dijkstra's algorithm.
        /// See: https://en.wikipedia.org/wiki/Dijkstra%27s_algorithm#Pseudocode
        /// </summary>
        /// <param name="source">Starting point of the path</param>
        /// <param name="target">Endpoint of the path</param>
        /// <returns>A list of the connections necessary to traverse the graph on the shortest path from source to target</returns>
        IList<INetworkConnection> GetPathToNode(INetworkNode source, INetworkNode target);
    }
}