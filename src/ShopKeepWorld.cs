using Godot;
using System;
using System.Collections.Generic;

public partial class ShopKeepWorld : Node2D {
	Queue<Player> playersQueue = new Queue<Player>();
	[Export]
	int numberOfPlayers = 5;
	PackedScene playerScene;
	Texture2D genericTexture;
	Control playerPos;
	DialogueBox dialogueBox;
	Player currentPlayer;
    Random randomNumGen;
	public override void _Ready() {
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
		dialogueBox.dialogue.Enqueue(currentPlayer.name + ": " + Dialogue.getGreeting(currentPlayer.personality));
	}

	public override void _Process(double delta) {
		if(Input.IsActionJustReleased("MouseClicked")) {

		}
	}
}
