using System;
using System.Collections.Generic;

using Godot;

using Soteria.Foundation.Contracts;

namespace Soteria.Foundation
{
    public abstract class NetworkNodeBase : Node2D, INetworkNode
    {
        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        protected readonly float ChanceToSpawnVirus = 0.0f;

        [Export(PropertyHint.Range, "0.0,1.0,0.05")]
        protected readonly float ThreatResistance = 0.8f;

        [Export(PropertyHint.ColorNoAlpha)]
        protected readonly Color NormalColor = new Color("003F7A");

        [Export(PropertyHint.ColorNoAlpha)]
        protected readonly Color InfectedColor = new Color("7D1022");

        protected readonly Random Randomizer = new Random();

        protected INetworkGraph NetworkGraph;

        public IList<INetworkConnection> Connections { get; private set; }

        public IList<IThreat> Infections { get; private set; }

        protected Label nodeTypeLabel;

        public void AddConnection(INetworkConnection connection)
        {
            if (!this.Connections.Contains(connection))
            {
                this.Connections.Add(connection);
            }
        }

        public override void _Ready()
        {
            base._Ready();

            this.Connections = new List<INetworkConnection>();
            this.Infections = new List<IThreat>();

            this.GetNode<Label>("CanvasLayer/ContextMenu/VBoxContainer/Label").Text = this.Name;

            this.NetworkGraph = this.GetNode<INetworkGraph>(new NodePath(".."));
            this.NetworkGraph.NetworkTick += this.NetworkGraph_OnNetworkTick;

            
            this.nodeTypeLabel = this.GetNode<Label>("CanvasLayer/NodeTypeLabel");
            this.nodeTypeLabel.MarginLeft = this.Position.x;
            this.nodeTypeLabel.MarginTop = this.Position.y;
            this.nodeTypeLabel.Hide();
        }

        protected void _on_Area2D_input_event(Node viewport, InputEvent @event, int shapeIdx)
        {
            if (@event is InputEventMouseButton button && button.ButtonIndex == (int)ButtonList.Left)
            {
                this.nodeTypeLabel.Hide();
                var contextMenu = this.GetNode<PopupPanel>("CanvasLayer/ContextMenu");

                var visibleRect = this.GetTree().Root.GetVisibleRect();

                // TODO Refactor this later to not include magic numbers
                // We also need to substract anything that is in the UI, which is 50px off the bottom and 215px from the right side
                var maxVector = visibleRect.Position + visibleRect.Size - contextMenu.RectSize - new Vector2(215, 50);

                contextMenu.MarginLeft = Mathf.Min(this.Position.x, maxVector.x);
                contextMenu.MarginTop = Mathf.Min(this.Position.y, maxVector.y);

                contextMenu.Popup_();
            }
        }
        
        protected void _on_Area2D_mouse_entered()
        {
            this.nodeTypeLabel.Show();
        }
        protected void _on_Area2D_mouse_exited()
        {
            this.nodeTypeLabel.Hide();
        }

        protected abstract void NetworkGraph_OnNetworkTick(object sender, EventArgs e);

        public abstract bool AttemptInfection(IThreat threat);
    }
}