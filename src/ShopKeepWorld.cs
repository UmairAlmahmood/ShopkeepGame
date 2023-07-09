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
	float moneyEarned = 0.00f;
	float currentWillingness = 0;
	Item currentItem;
	RichTextLabel coinsEarned;
	public override void _Ready() {
		coinsEarned = GetNode<RichTextLabel>("/root/World/CanvasLayer/CoinsAmount");
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
			player.SetMeta("Personality", randomNumGen.Next(1, 5));
			PlayerClass playerClass = (PlayerClass)randomNumGen.Next(1, 4);
			player.SetMeta("Class", (int)playerClass);
			player.SetMeta("SpecialTrait", randomNumGen.Next(1, 6));
			genericTexture = playerClass switch {
				PlayerClass.Warrior => ResourceLoader.Load<Texture2D>("res://assets/Player Art/warrior-texture.png"),
				PlayerClass.Archer => ResourceLoader.Load<Texture2D>("res://assets/Player Art/archer-texture.png"),
				PlayerClass.Mage => ResourceLoader.Load<Texture2D>("res://assets/Player Art/mage-texture.png"),
				_ => ResourceLoader.Load<Texture2D>("res://assets/ItemImages/GenericJellyItem.png"),
			};
			player.SetMeta("Portrait", genericTexture);
			player.SetMeta("Budget", randomNumGen.NextDouble()*10000);
			playersQueue.Enqueue(player);
		}
		currentPlayer = playersQueue.Dequeue();
		playerPos.AddChild(currentPlayer);
		Vector2 newPos = currentPlayer.Position;
		Vector2 startPos = currentPlayer.Position;
		startPos.X = -GetViewportRect().Size.X;
		currentPlayer.Position = startPos;
		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(currentPlayer, "position", newPos, 1.0);
		tween.TweenProperty(dialogueBox, "modulate", originalColor, 1.0);
		dialogueBox.dialogue.Enqueue(currentPlayer.name + ": " + Dialogue.getGreeting(currentPlayer.personality, currentPlayer.specialTrait));

		tween.Finished += () => {
			dialogueBox.setText();
            dialoguePicker.SetOptions(new List<(string, Action)>{
            ("I see, let me show you something", () => {
                EmitSignal(SignalName.SentReaction, "I see, let me show you something", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            }),
            ("Hello, I will have the item you want", () => {
                EmitSignal(SignalName.SentReaction, "Hello, I will have the item you want", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            }),
            ("I might have what you are looking for", () => {
                EmitSignal(SignalName.SentReaction, "I might have what you are looking for", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            })

            });
        };
	}

    private async void itemSentHandler(Item item) {
		float willingness = currentPlayer.CalculatePurchaseWillingess(item, 0);
		currentWillingness = willingness;
		currentItem = item;
		dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + Dialogue.getDialogueFromWillingess(willingness));
		dialogueBox.setText();
		bool quit = currentPlayer.modifyQuitPercentage(willingness);
		if(quit) {
			dialogueBox.dialogue.Enqueue("\n" + currentPlayer.name + ": It seems you haven't got what I want, so I will carry on my search");
			cyclePlayer();
			return;
		}
		timer.Start(.5);
		await ToSignal(timer, "timeout");
		dialoguePicker.SetOptions(new List<(string, Action)>{
		("Are you willing to buy?", () => {
			EmitSignal(SignalName.SentReaction, "Are you willing to buy?", (int)ReactionType.SellItem);
			dialoguePicker.Hide();
		}),
		("I see another item that you may be interested in", () => {
			EmitSignal(SignalName.SentReaction, "I see another item that you may be interested in", (int)ReactionType.ShowItem);
			dialoguePicker.Hide();
		}),
		});
    }
	
	public enum ReactionType {
		ShowItem, SellItem, Continue, ChangePrice,
	}

    private async void reactionHandler(string text, ReactionType reactionType) {
		dialogueBox.dialogue.Enqueue("\n\nMe: " + text);
		dialogueBox.setText();
		timer.Start(1.0);
		await ToSignal(timer, "timeout");
		if(reactionType == ReactionType.ShowItem) {
            dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + "Well let's see what you have in store");
            dialogueBox.setText();
		} else if(reactionType == ReactionType.SellItem) {
			(BuyEnum buys, String message) = currentPlayer.buy(currentWillingness, currentItem.cost);
            dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + message);
            dialogueBox.setText();
			if(buys == BuyEnum.WillBuy) {
				moneyEarned += inventory.ItemSold();
				coinsEarned.Text = "Money: $" + Math.Round(moneyEarned, 2);
				cyclePlayer();
			} else if (buys == BuyEnum.TooExpensive) {
				dialoguePicker.SetOptions( new List<(string, Action)> {
					("I can lower the price", () => {
						EmitSignal(SignalName.SentReaction, "I can lower the price", (int)ReactionType.ChangePrice);
						dialoguePicker.Hide();
					}),
					("I see another item that you may be interested in", () => {
						EmitSignal(SignalName.SentReaction, "I see another item that you may be interested in", (int)ReactionType.ShowItem);
						dialoguePicker.Hide();
					}),
				});
				
			} else {
				
			}
		} else if(reactionType == ReactionType.ChangePrice) {
            dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + "Very well change the price");
            dialogueBox.setText();
			await ToSignal(inventory, "PriceChangedAgain");

			(BuyEnum buys, String message) = currentPlayer.buy(currentWillingness, currentItem.cost);
			if(buys == BuyEnum.WillBuy) {
                dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + message);
                dialogueBox.setText();
				moneyEarned += inventory.ItemSold();
				coinsEarned.Text = "Money: $" + Math.Round(moneyEarned, 2);
				cyclePlayer();
			} else if(buys == BuyEnum.TooExpensive) {
                dialogueBox.dialogue.Enqueue("\n\n" + currentPlayer.name + ": " + "My friend it is still too expensive");
                dialogueBox.setText();
				dialoguePicker.SetOptions( new List<(string, Action)> {
					("I can lower the price further", () => {
						EmitSignal(SignalName.SentReaction, "I can lower the price further", (int)ReactionType.ChangePrice);
						dialoguePicker.Hide();
					}),
					("I see another item that you may be interested in", () => {
						EmitSignal(SignalName.SentReaction, "I see another item that you may be interested in", (int)ReactionType.ShowItem);
						dialoguePicker.Hide();
					}),
				});
				
			} 
		}
    }
	
	async void cyclePlayer() {
        Tween tween = GetTree().CreateTween();
        tween.SetTrans(Tween.TransitionType.Sine);
        Vector2 newPos = currentPlayer.Position;
        newPos.X = GetViewportRect().Size.X;
        tween.TweenProperty(currentPlayer, "position", newPos, 1.0f);
        await ToSignal(tween, "finished");
        currentPlayer.QueueFree();

		currentPlayer = playersQueue.Dequeue();
		playerPos.AddChild(currentPlayer);
		Vector2 originalPos = currentPlayer.Position;
		Vector2 startPos = currentPlayer.Position;
		startPos.X = -GetViewportRect().Size.X;
		currentPlayer.Position = startPos;
		Tween tween1 = GetTree().CreateTween();
		tween1.TweenProperty(currentPlayer, "position", originalPos, 1.0);
		dialogueBox.Reset();
		dialogueBox.dialogue.Enqueue(currentPlayer.name + ": " + Dialogue.getGreeting(currentPlayer.personality, currentPlayer.specialTrait));
        dialogueBox.setText();
        dialoguePicker.SetOptions(new List<(string, Action)>{
            ("I see, let me show you something", () => {
                EmitSignal(SignalName.SentReaction, "I see, let me show you something", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            }),
            ("Hello, I will have the item you want", () => {
                EmitSignal(SignalName.SentReaction, "Hello, I will have the item you want", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            }),
            ("I might have what you are looking for", () => {
                EmitSignal(SignalName.SentReaction, "I might have what you are looking for", (int)ReactionType.ShowItem);
                dialoguePicker.Hide();
            })
		});
		inventory.returnItemsToInv();
	}

    public override void _Process(double delta) {

	}

	[Signal]
	public delegate void SentReactionEventHandler(String text, ReactionType reactionType);
}
