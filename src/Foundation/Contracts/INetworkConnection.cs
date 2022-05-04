namespace Soteria.Foundation.Contracts
{
    public interface INetworkConnection
    {
        INetworkNode Source { get; }

        INetworkNode Target { get; }
    }
}