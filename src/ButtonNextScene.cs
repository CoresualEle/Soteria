using Godot;
using System;

public class ButtonNextScene : Button
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("pressed", this, nameof(ButtonPressed));
	}

	public void ButtonPressed()
	{
		GD.Print("Going to SubScene");
		GetTree().ChangeScene("res://SubScene/SubScene.tscn");
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	//  public override void _Process(float delta)
	//  {
	//      
	//  }
}
