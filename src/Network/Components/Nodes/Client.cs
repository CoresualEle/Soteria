using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components.Nodes
{
    public class Client : NetworkNodeBase
    {
        public override bool AttemptInfection(IThreat threat)
        {
            this.GameVariables.AttemptedInfections += 1;
            if (!this.Infections.Contains(threat) && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                this.GameVariables.SuccessfulInfections += 1;
                this.Infections.Add(threat);
                this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;

                return true;
            }

            return false;
        }
    }
}