using System.Collections.Generic;

using Godot;

namespace Soteria.Foundation.Contracts
{
    public interface INetworkNode
    {
        string Name { get; }

        IList<INetworkConnection> Connections { get; }

        IList<IThreat> Infections { get; }

        Vector2 Position { get; }

        void AddConnection(INetworkConnection connection);

        bool AttemptInfection(IThreat threat);
    }
}