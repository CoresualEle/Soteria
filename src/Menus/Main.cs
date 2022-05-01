using Godot;
using System;

namespace Menus {
	public class Main : Control
	{
		public override void _Ready()
		{
			GetTree().Root.Connect("size_changed", this, nameof(Resize));

			GetNode<Button>("VBoxContainer/StartButton").GrabFocus();
		}

		private void _on_StartButton_pressed()
		{
			//GetTree().ChangeScene("res://Menus/OptionsMenu.tscn");
		}

		private void _on_OptionsButton_pressed()
		{
			GetNode<VBoxContainer>("VBoxContainer").Hide();

			var options_scene = (PackedScene)ResourceLoader.Load("res://Menus/Options.tscn");
			var options_scene_instance = (Control)options_scene.Instance();

			GetTree().CurrentScene.AddChild(options_scene_instance);

			options_scene_instance.Connect("BackButtonPressed", this, "_on_Options_BackButton_Signal");
		}

		private void Resize()
		{
			var visibleRect = GetTree().Root.GetVisibleRect();
			var background = GetNode<TextureRect>("TextureRect");
			background.RectPosition = visibleRect.Position;
			background.RectSize = visibleRect.Size;
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
