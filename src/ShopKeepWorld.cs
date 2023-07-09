using Godot;
using System;
using System.Collections.Generic;

public partial class ShopKeepWorld : Node2D {
	Queue<Player> playersQueue = new Queue<Player>();
	[Export]
	int numberOfPlayers = 5;
	PackedScene playerScene;
	DialoguePicker dialoguePicker;
	Texture2D genericTexture;
	Control playerPos;
	DialogueBox dialogueBox;
	Player currentPlayer;
    Random randomNumGen;
	public override void _Ready() {
		dialoguePicker = GetNode<DialoguePicker>("CanvasLayer/DialoguePicker");
		dialoguePicker.Hide();
		SentReaction += reactionHandler;

		dialogueBox = GetNode<DialogueBox>("CanvasLayer/DialogueBox");
		Color originalColor = dialogueBox.Modulate;
		Color newColor = originalColor;
		newColor.A = 0.0f;
		dialogueBox.Modulate = newColor;
		playerPos = GetNode<Control>("CanvasLayer/PlayerPos");

		genericTexture = ResourceLoader.Load<Texture2D>("res://assets/ItemImages/GenericJellyItem.png");
		playerScene = ResourceLoader.Load<PackedScene>("res://players/Player.tscn");
		randomNumGen = new Random();
		for(int i = 0; i<numberOfPlayers; i++) {
			Player player = playerScene.Instantiate<Player>();
			player.SetMeta("Name", Player.generatePlayerName());
			player.SetMeta("Portrait", genericTexture);
			player.SetMeta("Personality", randomNumGen.Next(1, 5));
			player.SetMeta("Class", randomNumGen.Next(1, 4));
			player.SetMeta("SpecialTrait", randomNumGen.Next(1, 6));
			playersQueue.Enqueue(player);
		}
		currentPlayer = playersQueue.Dequeue();
		playerPos.AddChild(currentPlayer);
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(dialogueBox, "modulate", originalColor, 1.5f);
		dialogueBox.dialogue.Enqueue(currentPlayer.name + ": " + Dialogue.getGreeting());

		tween.Finished += () => {
			dialogueBox.setText();
            dialoguePicker.SetOptions(new List<(string, Action)>{
            ("I see", () => {
                EmitSignal(SignalName.SentReaction, "I see");
                dialoguePicker.Hide();
            }),
            ("Hello", () => {
                EmitSignal(SignalName.SentReaction, "Hello");
                dialoguePicker.Hide();
            }),
            ("I might have what you are looking for", () => {
                EmitSignal(SignalName.SentReaction, "I might have what you are looking for");
                dialoguePicker.Hide();
            })
			});
		};
	}

    private void reactionHandler(string text) {
		dialogueBox.dialogue.Enqueue("\n\nMe: " + text);
		dialogueBox.setText();
    }

    public override void _Process(double delta) {

	}

	[Signal]
	public delegate void SentReactionEventHandler(String text);
}
