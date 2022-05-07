namespace Soteria.Foundation.Contracts
{
    public interface INetworkConnection
    {
        int Weight { get; }

        INetworkNode Source { get; }

        INetworkNode Target { get; }
    }
}