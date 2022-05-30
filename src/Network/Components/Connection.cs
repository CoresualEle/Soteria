using System;
using System.Linq;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components
{
    public class Connection : Node2D, INetworkConnection
    {
        private readonly Color normalColor = new Color("003566");
        private readonly Color infectedColor = new Color("7D1022");
        private readonly Color infectableColor = new Color("#7A3D00");
        private Line2D lineGraphic;

        [Export]
        public NodePath SourceNodePath;

        [Export]
        public NodePath TargetNodePath;

        public int Weight
        {
            get
            {
                return 1;
            }
        }

        public INetworkNode Source { get; private set; }

        public INetworkNode Target { get; private set; }

        public override void _Ready()
        {
            this.lineGraphic = (Line2D)this.GetNode(new NodePath("./ConnectionGraphic"));

            this.Source = (INetworkNode)this.GetNode(this.SourceNodePath) ?? throw new ArgumentNullException(nameof(this.Source));
            this.Target = (INetworkNode)this.GetNode(this.TargetNodePath) ?? throw new ArgumentNullException(nameof(this.Target));

            this.GetNode<INetworkGraph>(new NodePath("..")).RegisterConnection(this);

            this.SetupConnectionGraphic();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            this.SetupConnectionGraphic();
        }

        private void SetupConnectionGraphic()
        {
            if (this.Source.Infections != null && this.Target.Infections != null)
            {
                if (!this.Source.Infections.Any())
                {
                    this.lineGraphic.DefaultColor = this.normalColor;
                }
                else if (this.Source.Infections.Any() && !this.Target.Infections.Any())
                {
                    this.lineGraphic.DefaultColor = this.infectableColor;
                }
                else if (this.Source.Infections.Any() && this.Target.Infections.Any())
                {
                    this.lineGraphic.DefaultColor = this.infectedColor;
                }
            }

            this.lineGraphic.ClearPoints();

            this.lineGraphic.AddPoint(new Vector2(this.Source.Position.x, this.Source.Position.y));
            this.lineGraphic.AddPoint(new Vector2(this.Target.Position.x, this.Target.Position.y));
        }
    }
}