using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueBox : Control {
    public Queue<String> dialogue = new Queue<String>();
    Label text;
    ScrollContainer scrollContainer;
    VBoxContainer vbox;
    public override void _Ready() {
        text = GetNode<Label>("TextureRect/MarginContainer/ScrollContainer/VBoxContainer/Text");
        scrollContainer = GetNode<ScrollContainer>("TextureRect/MarginContainer/ScrollContainer");
        vbox = GetNode<VBoxContainer>("TextureRect/MarginContainer/ScrollContainer/VBoxContainer");
        vbox.MinimumSizeChanged += () => {
            scrollContainer.ScrollVertical = (int)(scrollContainer.GetVScrollBar().MaxValue);
        };
        base._Ready();
    }

    public override void _Process(double delta) {
        if(Input.IsActionJustReleased("MouseClicked") && dialogue.Count != 0) {
            setText();
        }
        base._Process(delta);
    }
    
    public void setText() {
        if(dialogue.Count == 0) return;
        String nextDialogue = dialogue.Dequeue();
        text.Text += nextDialogue;
        scrollContainer.ScrollVertical = (int)(scrollContainer.GetVScrollBar().MaxValue - 20);
    }

    public void Reset() {
        text.Text = "";
    }
}
