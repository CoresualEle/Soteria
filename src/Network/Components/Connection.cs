using System;

using Godot;

using Soteria.Foundation.Contracts;

public class Connection : Node2D, INetworkConnection
{
    private Line2D lineGraphic;

    [Export]
    public NodePath SourceNodePath;

    [Export]
    public NodePath TargetNodePath;

    public INetworkNode Source { get; private set; }

    public INetworkNode Target { get; private set; }

    public override void _Ready()
    {
        this.lineGraphic = (Line2D)this.GetNode(new NodePath("./ConnectionGraphic"));

        this.Source = (INetworkNode)this.GetNode(this.SourceNodePath) ?? throw new ArgumentNullException(nameof(this.Source));
        this.Target = (INetworkNode)this.GetNode(this.TargetNodePath) ?? throw new ArgumentNullException(nameof(this.Target));


        this.SetupConnectionGraphic();
    }

    private void SetupConnectionGraphic()
    {

        this.lineGraphic.ClearPoints();

        this.lineGraphic.AddPoint(new Vector2(this.Source.Position.x, this.Source.Position.y));
        this.lineGraphic.AddPoint(new Vector2(this.Target.Position.x, this.Target.Position.y));
    }
}