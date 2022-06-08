using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation.Contracts;
using Soteria.Network;

namespace Soteria.Foundation
{
    public abstract class SpreadingThreatBase : Node, IThreat
    {
        [Export(PropertyHint.Range, "0,14,1,or_greater")]
        public readonly int DaysBetweenInfections = 0;

        protected GameVariables GameVariables;
        protected NetworkGraph NetworkGraph;
        protected List<INetworkNode> InfectedNodes;
        protected int DayCounter;
        protected INetworkNode NodeToInfect;

        [Export]
        public NodePath NodeToInfectPath;

        public override void _Ready()
        {
            base._Ready();

            this.GameVariables = this.GetNode<GameVariables>("/root/GameVariables");
            this.NetworkGraph = this.GetNode<NetworkGraph>("..");
            this.InfectedNodes = new List<INetworkNode>();

            this.NodeToInfect = (INetworkNode)this.GetNode(this.NodeToInfectPath);

            this.NetworkGraph.NetworkTick += this.OnNetworkTick;

            this.DayCounter = this.DaysBetweenInfections;
        }

        public void OnNetworkTick(object sender, EventArgs e)
        {
            if (this.DayCounter < 0)
            {
                this.DayCounter = this.DaysBetweenInfections;
                this.Spread();
            }
            else
            {
                this.DayCounter -= 1;
            }
        }

        protected abstract void Spread();

        public abstract void RemoveNode(INetworkNode node);
    }
}