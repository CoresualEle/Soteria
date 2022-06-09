namespace Soteria.Foundation.Contracts
{
    public interface IThreat
    {
        string Name {get; }
        void RemoveNode(INetworkNode node);
    }
}