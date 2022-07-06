using System.Collections.Generic;

using Godot;

/* Based on https://github.com/benbishopnz/godot-credits/blob/master/GodotCredits.gd */

public class Credits : Node2D
{
    private readonly float sectionTime = 1.5f;
    private readonly float lineTime = 0.5f;
    private readonly float baseSpeed = 0.7f;
    private readonly Color titleColor = Colors.White;
    private readonly List<Label> lines = new List<Label>();

    private readonly List<List<string>> credits = new List<List<string>>
    {
        new List<string>
        {
            "A game by",
            "Anatol Anciferov",
            "Semjon Alexander Bibow",
            "Rico Clemens",
            "Ferhat Güner",
            "Frederic Oldenbüttel"
        },
        new List<string> { "Game Design", "Semjon Alexander Bibow", "Rico Clemens" },
        new List<string> { "Programming", "Anatol Anciferov", "Semjon Alexander Bibow", "Frederic Oldenbüttel" },
        new List<string> { "Test manager", "Ferhat Güner" },
        new List<string> { "Sound effects", "Ferhat Güner" },
        new List<string> { "Music supervisor", "Ferhat Güner" },
        new List<string>
        {
            "Tools used",
            "Developed with Godot Engine",
            "Licensed under MIT License",
            "https://godotengine.org/license",
            "",
            "Icons from Material Icons",
            "Licensed under Apache License 2.0",
            "https://developers.google.com/fonts/docs/material_icons#licensing",
            "",
            "Dongle Font",
            "Designed by Yanghee Ryu",
            "Licensed under SIL Open Font License 1.1",
            "https://github.com/yangheeryu/Dongle/blob/master/OFL.txt"
        },
        new List<string> { "Special thanks", "TODO" },
    };

    private List<string> section = new List<string>();
    private bool sectionNext = true;
    private float sectionTimer;
    private float lineTimer;
    private int currentLine;

    private float scrollSpeed;

    private bool started;

    private Label line;
    private Control creditsContainer;

    [Signal]
    public delegate void CreditsFinished();

    public override void _Ready()
    {
        this.creditsContainer = this.GetNode<Control>("CreditsContainer");
        this.line = this.GetNode<Label>("CreditsContainer/Line");

        this.scrollSpeed = this.baseSpeed;
    }

    private void Finished()
    {
        this.EmitSignal(nameof(CreditsFinished));
        this.QueueFree();
    }

    public override void _Process(float delta)
    {
        if (this.sectionNext)
        {
            this.sectionTimer += delta;
            if (this.sectionTimer >= this.sectionTime)
            {
                this.sectionTimer -= this.sectionTime;

                if (this.credits.ToArray().Length > 0)
                {
                    this.started = true;
                    this.section = this.credits[0];
                    this.credits.RemoveAt(0);
                    this.currentLine = 0;
                    this.AddLine();
                }
            }
        }
        else
        {
            this.lineTimer += delta;
            if (this.lineTimer >= this.lineTime)
            {
                this.lineTimer -= this.lineTime;
                this.AddLine();
            }
        }

        if (this.lines.ToArray().Length > 0)
        {
            foreach (var l in this.lines.ToArray())
            {
                l.RectPosition += new Vector2(0, -this.scrollSpeed);
                if (l.RectPosition.y < -l.GetLineHeight())
                {
                    this.lines.Remove(l);
                    l.QueueFree();
                }
            }
        }
        else
        {
            if (this.started)
            {
                this.Finished();
            }
        }
    }

    private void AddLine()
    {
        var newLine = (Label)this.line.Duplicate();
        newLine.Text = this.section[0];
        this.section.RemoveAt(0);
        this.lines.Add(newLine);

        if (this.currentLine == 0)
        {
            newLine.AddColorOverride("font_color", this.titleColor);
        }

        this.creditsContainer.AddChild(newLine);

        if (this.section.ToArray().Length > 0)
        {
            this.currentLine += 1;
            this.sectionNext = false;
        }
        else
        {
            this.sectionNext = true;
        }
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
        {
            this.Finished();
        }
    }
}