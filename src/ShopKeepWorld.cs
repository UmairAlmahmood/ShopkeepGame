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
	Control dialogueBox;
	public override void _Ready() {
		dialogueBox = GetNode<Control>("CanvasLayer/DialogueBox");
		Color originalColor = dialogueBox.Modulate;
		Color newColor = originalColor;
		newColor.A = 0.0f;
		dialogueBox.Modulate = newColor;
		playerPos = GetNode<Control>("CanvasLayer/PlayerPos");

		genericTexture = ResourceLoader.Load<Texture2D>("res://assets/ItemImages/GenericJellyItem.png");
		playerScene = ResourceLoader.Load<PackedScene>("res://players/Player.tscn");
		Random randomNumGen = new Random();
		for(int i = 0; i<numberOfPlayers; i++) {
			Player player = playerScene.Instantiate<Player>();
			player.SetMeta("Name", "John Smith");
			player.SetMeta("Portrait", genericTexture);
			player.SetMeta("Personality", randomNumGen.Next(1, 3));
			player.SetMeta("Class", randomNumGen.Next(1, 4));
			playersQueue.Enqueue(player);
		}
		playerPos.AddChild(playersQueue.Dequeue());
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(dialogueBox, "modulate", originalColor, 1.5f);
	}

	public override void _Process(double delta) {
	}
}
