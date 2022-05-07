using System.Collections.Generic;

using Godot;

using Soteria.Foundation.Contracts;

public class Server : Node2D, INetworkNode
{
    public IList<INetworkConnection> Connections { get; } = new List<INetworkConnection>();

    public override void _Ready()
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