using Godot;
using System;

namespace Menus {
public class Main : Control
{
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/StartButton").GrabFocus();
	}

	private void _on_StartButton_pressed()
	{
		GetTree().ChangeScene("res://Maps/World.tscn");
	}

	private void _on_OptionsButton_pressed()
	{
		GetNode<VBoxContainer>("VBoxContainer").Hide();

		var options_scene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
		var options_scene_instance = (Control)options_scene.Instance();

		GetTree().CurrentScene.AddChild(options_scene_instance);

		options_scene_instance.Connect("BackButtonPressed", this, "_on_Options_BackButton_Signal");
	}

	private void _on_Options_BackButton_Signal()
	{
		GetNode<VBoxContainer>("VBoxContainer").Show();
		GetNode<Button>("VBoxContainer/OptionsButton").GrabFocus();
	}

	private void _on_QuitButton_pressed()
	{
		GetTree().Quit();
	}
}
}
