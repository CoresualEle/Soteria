using System;
using System.Linq;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Network.Components
{
    public class Connection : Node2D, INetworkConnection
    {
        private float frame;
        private readonly Color[] colorsNormal = { new Color("#005FB8"), new Color("#007EF5"), new Color("#005FB8")};
        private readonly Color[] colorsInfectable= { new Color("#A35200"), new Color("#F57A00"), new Color("#A35200")};
        private readonly Color[] colorsInfected = { new Color("#901328"), new Color("#E32646"), new Color("#901328")};
        
        private Vector2 offsetx1 = new Vector2(1,0);
        private Vector2 offsetx2 = new Vector2(-1,0);
        private Vector2 offsety1 = new Vector2(0,1);
        private Vector2 offsety2 = new Vector2(0,-1);

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
            this.Source = (INetworkNode)this.GetNode(this.SourceNodePath) ?? throw new ArgumentNullException(nameof(this.Source));
            this.Target = (INetworkNode)this.GetNode(this.TargetNodePath) ?? throw new ArgumentNullException(nameof(this.Target));

            this.GetNode<INetworkGraph>(new NodePath("..")).RegisterConnection(this);

            // Randomize starting point of animation
            this.frame = (float) new Random().NextDouble();
        }

        public override void _Process(float delta)
        {
            base._Process(delta);

            // Calls the _Draw() method
            this.Update();
        }

        public override void _Draw()
        {
            var newValue = (this.frame + 0.01f);
            this.frame = newValue > 1.0f ? 0f : newValue;
            var directionVector = this.Target.Position - this.Source.Position;
            
            Vector2[] points = {
                this.Source.Position + this.frame * directionVector,
                this.Source.Position + (this.frame + 0.1f) * directionVector,
                this.Source.Position + (this.frame + 0.2f) * directionVector
            };
            Vector2[] points1 = {
                points[0] + this.offsetx1,
                points[1] + this.offsetx1,
                points[2] + this.offsetx1
            };
            Vector2[] points2 = {
                points[0] + this.offsetx2,
                points[1] + this.offsetx2,
                points[2] + this.offsetx2
            };
            Vector2[] points3 = {
                points[0] + this.offsety1,
                points[1] + this.offsety1,
                points[2] + this.offsety1
            };
            Vector2[] points4 = {
                points[0] + this.offsety2,
                points[1] + this.offsety2,
                points[2] + this.offsety2
            };

            if (this.Source.Infections != null && this.Target.Infections != null)
            {
                if (!this.Source.Infections.Any())
                {
                    this.DrawMultilineColors(points, this.colorsNormal);
                    this.DrawMultilineColors(points1, this.colorsNormal);
                    this.DrawMultilineColors(points2, this.colorsNormal);
                    this.DrawMultilineColors(points3, this.colorsNormal);
                    this.DrawMultilineColors(points4, this.colorsNormal);
                }
                else if (this.Source.Infections.Any() && !this.Target.Infections.Any())
                {
                    this.DrawMultilineColors(points, this.colorsInfectable);
                    this.DrawMultilineColors(points1, this.colorsInfectable);
                    this.DrawMultilineColors(points2, this.colorsInfectable);
                    this.DrawMultilineColors(points3, this.colorsInfectable);
                    this.DrawMultilineColors(points4, this.colorsInfectable);
                }
                else if (this.Source.Infections.Any() && this.Target.Infections.Any())
                {
                    this.DrawMultilineColors(points, this.colorsInfected);
                    this.DrawMultilineColors(points1, this.colorsInfected);
                    this.DrawMultilineColors(points2, this.colorsInfected);
                    this.DrawMultilineColors(points3, this.colorsInfected);
                    this.DrawMultilineColors(points4, this.colorsInfected);
                }
            }
        }

    }
}