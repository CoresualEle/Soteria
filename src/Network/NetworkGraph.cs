using System;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Network
{
    public class NetworkGraph : Node2D, INetworkGraph
    {
        public override void _Ready()
        {
        }

        public void RegisterConnection(INetworkConnection connectionToRegister)
        {
            throw new NotImplementedException();
        }
    }
}