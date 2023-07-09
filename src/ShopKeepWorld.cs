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
	Timer timer;
	Inventory inventory;
	public override void _Ready() {
		inventory = GetNode<Inventory>("CanvasLayer/Inventory");
		inventory.ItemSent += itemSentHandler;
		timer = GetNode<Timer>("/root/World/Timer");
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
		tween.TweenProperty(dialogueBox, "modulate", originalColor, 1.0f);
		dialogueBox.dialogue.Enqueue(currentPlayer.name + ": " + Dialogue.getGreeting(currentPlayer.personality, currentPlayer.playerClass, currentPlayer.specialTrait));

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

    private void itemSentHandler(Item item) {
		float willingness = currentPlayer.CalculatePurchaseWillingess(item, 0);
		dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + Dialogue.getDialogueFromWillingess(willingness));
		dialogueBox.setText();
		bool quit = currentPlayer.modifyQuitPercentage(willingness);
		if(quit) {
			dialogueBox.dialogue.Enqueue("\n" + currentPlayer.name + ": It seems you haven't got what I want, so I will carry on my search");
		}
    }

    private async void reactionHandler(string text) {
		dialogueBox.dialogue.Enqueue("\n\nMe: " + text);
		dialogueBox.setText();
		timer.Start(.5);
		await ToSignal(timer, "timeout");
		dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + "Well lets see what you have in store");
		dialogueBox.setText();
    }

    public override void _Process(double delta) {

	}

	[Signal]
	public delegate void SentReactionEventHandler(String text);
}
