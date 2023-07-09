using Godot;
using System;

public partial class StartMenu : Control {
    Button play;
    Button quit;
    PackedScene level;
    public override void _Ready() {
        level = ResourceLoader.Load<PackedScene>("res://ShopKeepWorld.tscn");
        play = GetNode<Button>("MarginContainer/VBoxContainer/Play");
        play.Pressed += () => {
            GetTree().ChangeSceneToPacked(level);
        };
        quit = GetNode<Button>("MarginContainer/VBoxContainer/Quit");
        quit.Pressed += () => GetTree().Quit();
        base._Ready();
    }
}
