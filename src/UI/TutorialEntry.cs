using System.Linq;

using Godot;

namespace Soteria.UI
{
    public class TutorialEntry : VBoxContainer
    {
        private readonly Color highlightColor = new Color("ff0000");
        private readonly Color darkenColor = new Color("B0000000"); //cc000000
        public NodePath NodeToHighlight;

        private Polygon2D highlighter;
        private Polygon2D border;
        private Polygon2D bg;

        public override void _Ready()
        {
            var vpSize = this.GetViewport().Size;

            if (!(this.NodeToHighlight is null))
            {
                if (this.NodeToHighlight.GetNameCount() > 1)
                {
                    var polygon = this.GetBoundingBox(this.NodeToHighlight);
                    this.highlighter = new Polygon2D
                    {
                        Color = this.highlightColor,
                        InvertEnable = true,
                        InvertBorder = 2,
                        Polygon = polygon
                    };
                    this.border = new Polygon2D
                    {
                        Color = this.darkenColor,
                        InvertEnable = true,
                        InvertBorder = Mathf.Max(vpSize.x, vpSize.y),
                        Polygon = polygon
                    };

                    this.AddChild(this.border);
                    this.AddChild(this.highlighter);

                    this.MoveChild(this.border, 0);
                    this.MoveChild(this.highlighter, 0);
                }
                else
                {
                    this.bg = new Polygon2D
                    {
                        Color = this.darkenColor,
                        InvertEnable = true,
                        InvertBorder = Mathf.Max(vpSize.x * 2f, vpSize.y * 2f),
                        Polygon = new[] { new Vector2(-1, -1), new Vector2(-1, -2), new Vector2(-2, -2) }
                    };

                    this.AddChild(this.bg);
                    this.MoveChild(this.bg, 0);
                }
            }
        }

        private Vector2[] GetBoundingBox(NodePath nodeToHighlight)
        {
            var node = this.GetNode(nodeToHighlight);

            var pos1 = new Vector2();
            var pos2 = new Vector2();

            if (node is Control cNode)
            {
                pos1 = cNode.RectGlobalPosition;
                pos2 = pos1 + cNode.RectSize;
            }
            else if (node is Polygon2D pNode)
            {
                var offset = pNode.GlobalPosition;
                var allPoints = pNode.Polygon.AsEnumerable();
                var xPoints = allPoints.Select(point => point.x);
                var yPoints = allPoints.Select(point => point.y);

                pos1 = new Vector2(xPoints.Min(), yPoints.Min()) + offset;
                pos2 = new Vector2(xPoints.Max(), yPoints.Max()) + offset;
            }

            var vpOffset = this.RectGlobalPosition;

            var points = new[]
            {
                pos1 - vpOffset,
                new Vector2(pos2.x, pos1.y) - vpOffset,
                pos2 - vpOffset,
                new Vector2(pos1.x, pos2.y) - vpOffset,
            };

            return points;
        }
    }
}