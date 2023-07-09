using Godot;
using System;
using System.Collections.Generic;

public partial class DialoguePicker : Control {
    public List<(String, Action)> options = new List<(String, Action)>();
    List<Button> buttons = new List<Button>();
    VBoxContainer vbox;
    PackedScene button;
    public override void _Ready() {
        button = ResourceLoader.Load<PackedScene>("res://scenes/ButtonOption.tscn");
        vbox = GetNode<VBoxContainer>("MarginContainer/VBoxContainer");
        base._Ready();
    }

    public void SetOptions(List<(String, Action)> options) {
        GD.Print(options[0], " ", options[1]);
        this.options.Clear();
        this.buttons.Clear();
        foreach(Node buttonOption in vbox.GetChildren())  {
            buttonOption.QueueFree();
        }
        this.options = options;
        foreach((String option, Action callback) in options) {
            GD.Print("A");
            Button buttonOption = button.Instantiate<Button>();
            buttons.Add(buttonOption);
            buttonOption.Text = option;
            buttonOption.Pressed += callback;
            vbox.AddChild(buttonOption);
        }
        Show();
    }
}
