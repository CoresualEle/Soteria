using Soteria.Foundation;
using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components.Nodes
{
    public class Internet : NetworkNodeBase
    {
        public override bool AttemptInfection(IThreat threat)
        {
            // The internet cannot be infected by anything
            return false;
        }
    }
}