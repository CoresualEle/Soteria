using Soteria.Foundation;
using Soteria.Foundation.Contracts;
using System;

namespace Soteria.Network.Components.Nodes
{
    public class Internet : NetworkNodeBase
    {
        //protected override void NetworkGraph_OnNetworkTick(object sender, EventArgs e)
        //{
        //    if (this.ChanceToSpawnVirus > 0 && this.Randomizer.NextDouble() <= this.ChanceToSpawnVirus && this.Randomizer.NextDouble() > this.ThreatResistance)
        //    {
        //        var newThreat = new SpreadingThreatBase(this, this.NetworkGraph);
        //        if (!this.Infections.Contains(newThreat))
        //        {
        //            this.Infections.Add(newThreat);
        //        }
        //    }
        //}

        public override bool AttemptInfection(IThreat threat)
        {
            return false;
        }
    }
}