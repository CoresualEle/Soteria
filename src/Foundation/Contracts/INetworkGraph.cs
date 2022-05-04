using System.Collections.Generic;

namespace Soteria.Foundation.Contracts
{
    public interface INetworkGraph
    {
        void RegisterConnection(INetworkConnection connectionToRegister);
    }
}