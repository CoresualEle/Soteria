using Godot;


public class Options : Control
{
	[Signal]
	public delegate void BackButtonPressed();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Button>("VBoxContainer/GraphicsButton").GrabFocus();
	}

	private void _on_GraphicsButton_pressed()
	{
		GetNode<VBoxContainer>("VBoxContainer").Hide();

		var options_graphics_scene = (PackedScene)ResourceLoader.Load("res://Menus/OptionsGraphics.tscn");
		var options_graphics_scene_instance = (Control)options_graphics_scene.Instance();

		GetTree().CurrentScene.AddChild(options_graphics_scene_instance);

		options_graphics_scene_instance.Connect("BackButtonPressed", this, "_on_Options_BackButton_Signal");
	}
	private void _on_Options_BackButton_Signal()
	{
		GetNode<VBoxContainer>("VBoxContainer").Show();
		GetNode<Button>("VBoxContainer/GraphicsButton").GrabFocus();
	}

	private void _on_BackButton_pressed()
	{
		QueueFree();
		EmitSignal(nameof(BackButtonPressed));
	}
}
