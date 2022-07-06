using System.Collections.Generic;
using System.Linq;

using Godot;

namespace Soteria.UI
{
    public class TutorialOverlay : CanvasLayer
    {
        [Export]
        private readonly List<NodePath> nodesToHighlight = new List<NodePath>();

        private readonly List<TutorialEntry> tutorials = new List<TutorialEntry>();
        private int currentTutorial = -1;
        private GameVariables gameVariables;

        [Signal]
        public delegate void TutorialDone();

        public override void _Ready()
        {
            this.gameVariables = this.GetNode<GameVariables>("/root/GameVariables");

            foreach (var node in this.GetChildren())
            {
                if (node is VBoxContainer)
                {
                    var tutorialEntry = (TutorialEntry)node;
                    this.tutorials.Add(tutorialEntry);
                    tutorialEntry.Hide();
                }
            }

            if (this.tutorials.Count != this.nodesToHighlight.Count)
            {
                GD.Print("Unequal lengths of tutorials vs nodes. No Highlighting will be available...");
            }
            else
            {
                foreach (var (tutorial, nodepath) in this.tutorials.Zip(this.nodesToHighlight, (a, b) => (a, b)))
                {
                    tutorial.NodeToHighlight = $"../{nodepath}";
                    tutorial._Ready();
                    tutorial.Hide();
                }
            }

            this.ShowNextTutorial();
            this.gameVariables.SetTimeScale(0);
        }

        private void ShowNextTutorial()
        {
            this.gameVariables.SetTimeScale(0);
            this.currentTutorial += 1;
            if (this.currentTutorial >= this.tutorials.Count)
            {
                this.EmitSignal(nameof(TutorialDone));
                return;
            }

            this.tutorials[this.currentTutorial].Show();
            if (this.currentTutorial > 0)
            {
                this.tutorials[this.currentTutorial - 1].Hide();
            }
        }

        public override void _Input(InputEvent @event)
        {
            if (@event.IsActionPressed("ui_accept"))
            {
                this.ShowNextTutorial();
            }
        }
    }
}