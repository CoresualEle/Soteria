using System;

using Godot;

using Soteria.Foundation;
using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components.Nodes
{
    public class Client : NetworkNodeBase
    {
        public override void _Ready()
        {
            base._Ready();
        }

        protected override void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        {
            if (this.ChanceToSpawnVirus > 0 && this.Randomizer.NextDouble() <= this.ChanceToSpawnVirus && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                var newThreat = new SpreadingThreatBase(this, this.NetworkGraph);
                if (!this.Infections.Contains(newThreat))
                {
                    this.Infections.Add(newThreat);
                    this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;
                }
            }
        }

        public override bool AttemptInfection(IThreat threat)
        {
            if (!this.Infections.Contains(threat) && this.Randomizer.NextDouble() > this.ThreatResistance)
            {
                this.Infections.Add(threat);
                this.GetNode<Polygon2D>("Polygon2D").Color = this.InfectedColor;

                return true;
            }

            return false;
        }
    }
}