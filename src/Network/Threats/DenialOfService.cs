using Godot;

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
        this.GameVariables.NodeAffectedByDenialOfService -= 1;
    }

    private void AddInfectedNode(INetworkNode networkNode)
    {
        GD.Print($"{networkNode.Name} infected by DoS");
        this.InfectedNodes.Add(networkNode);
        this.GameVariables.NodeAffectedByDenialOfService += 1;
    }
}