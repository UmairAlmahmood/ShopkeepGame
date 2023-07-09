using Godot;
using System;
using System.Runtime.ExceptionServices;


public partial class Player : Control {
	Texture2D portraitImage;
	public String name;
	public Personality personality;
	public PlayerClass playerClass;
	public Label classLabel;
	Label nameLabel;
	TextureRect portraitRect;
	public override void _Ready() {
		classLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Class");
		nameLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Name");
		portraitRect = GetNode<TextureRect>("VBoxContainer/Portrait");
		

		portraitImage = (Texture2D)GetMeta("Portrait");
		portraitRect.Texture = portraitImage;
		name = (String)GetMeta("Name");
		nameLabel.Text = name;
		playerClass = (PlayerClass)(int)GetMeta("Class");
		classLabel.Text = playerClass.ToString();

		personality = (Personality)(int)GetMeta("Personality");

		GD.Print(GetMeta("Name") + "'s willingness is to buy " + Inventory.itemsList[0].GetMeta("Name") + ": " + CalculatePurchaseWillingess(Inventory.itemsList[0]));
	}

	public override void _Process(double delta) {
	}

	public float CalculatePurchaseWillingess(Item item) {

		float willingness = 0.0f;

		if((int)GetMeta("Class") == (int)item.GetMeta("ItemType")) {
			willingness += 0.5f;
			GD.Print("class/item match (+0.5)");
		}

		if((int)item.GetMeta("ItemType") == (int)ItemType.Potion) {
			willingness += 0.15f;
		}

		if((int)item.GetMeta("ItemType") == (int)ItemType.Potion) {
			willingness += 0.15f;
		}

		if((int)GetMeta("Personality") == (int)Personality.Jolly) {
			willingness += 0.2f;
			GD.Print("Jolly (+0.2)");
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.3f;

			willingness += (float) (-(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/(int)item.GetMeta("Rarity")+1.0f)/1.5f;
		}
		if((int)GetMeta("Personality") == (int)Personality.Cheapskate) {
			willingness -= 0.3f;
			GD.Print("Cheapskate (-0.3)");			
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.3f;

			willingness += (float) -(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/(5*(int)item.GetMeta("Rarity"))+0.2f;
		}
		if((int)GetMeta("Personality") == (int)Personality.Foolhardy) {
			willingness += 0.2f;
			GD.Print("Foolhardy (+0.2)");

			willingness += (float) -(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/5*(int)item.GetMeta("Rarity")+0.2f;
		}
		if((int)GetMeta("Personality") == (int)Personality.Cowardly) {
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.5f;

			willingness += (float) (Math.Pow(Math.E, (double)(int)(item.GetMeta("Rarity")))-1.5f)/2;
		}

		return willingness;
	}

	public static String generatePlayerName() {
		String[] FirstNames = {"Edmund", "Thad", "Gaston", "John", "Fletcher", "Kenneth", "Jonathan"};
		String[] LastNames = {"Stoneroar", "Firemourn", "Heavyhorn", "Evenbreeze", "Sagetree", "Blueguard", "Greendane", "Stoneshine", "Regalkeep", "Fletcher", "Kingsguard"};

		Random randint = new Random();

		return FirstNames[randint.Next(0, FirstNames.Length)] + " " + LastNames[randint.Next(0, LastNames.Length)];
	}
}

public enum Personality {
	Jolly = 1, Cheapskate = 2, Foolhardy = 3, Cowardly = 4,
}

public enum PlayerClass {
	Warrior = 1, Archer = 2, Mage = 3,
}

public enum SpecialTrait {
	VsUndead = 1, VsGoblins = 2, Dungeoneer = 3, Diver = 4, VsDragon = 5, CurseHunter = 6,
}

