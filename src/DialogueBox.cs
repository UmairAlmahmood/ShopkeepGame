using Godot;
using System;
using System.Collections.Generic;

public partial class DialogueBox : Control {
    public Queue<String> dialogue = new Queue<string>();
    Label text;
    public override void _Ready() {
        text = GetNode<Label>("TextureRect/MarginContainer/Text");
        base._Ready();
    }

    public override void _Process(double delta) {
        if(Input.IsActionJustReleased("MouseClicked") && dialogue.Count != 0) {
            String nextDialogue = dialogue.Dequeue();
            text.Text = nextDialogue;
        }
        base._Process(delta);
    }
}
