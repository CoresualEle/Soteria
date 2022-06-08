using Soteria.Foundation;
using Soteria.Foundation.Contracts;

public class DenialOfService : SpreadingThreatBase
{
    protected override void Spread()
    {
        if (!this.InfectedNodes.Contains(this.NodeToInfect) && this.NodeToInfect.AttemptInfection(this))
        {
            this.AddInfectedNode(this.NodeToInfect);
        }
    }

    public override void RemoveNode(INetworkNode node)
    {
        this.InfectedNodes.Remove(node);
        this.GameVariables.NodesAffectedByDenialOfService -= 1;
    }

    private void AddInfectedNode(INetworkNode networkNode)
    {
        this.InfectedNodes.Add(networkNode);
        this.GameVariables.NodesAffectedByDenialOfService += 1;
    }
}