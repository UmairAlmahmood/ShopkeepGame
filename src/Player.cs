using Godot;
using System;
using System.Runtime.ExceptionServices;


public partial class Player : Control {
	Texture2D portraitImage;
	public String name;
	public Personality personality;
	public PlayerClass playerClass;
	public Label classLabel;
	public SpecialTrait specialTrait;
	Label nameLabel;
	TextureRect portraitRect;
	float quitPercentage = 0.0f;
	float budget = 0.00f;
	public override void _Ready() {
		classLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Class");
		nameLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Name");
		portraitRect = GetNode<TextureRect>("VBoxContainer/Portrait");
		

		portraitImage = (Texture2D)GetMeta("Portrait");
		portraitRect.Texture = portraitImage;
		SetAnchorsPreset(LayoutPreset.Center);

		name = (String)GetMeta("Name");
		nameLabel.Text = name;
		playerClass = (PlayerClass)(int)GetMeta("Class");
		classLabel.Text = playerClass.ToString();

		personality = (Personality)(int)GetMeta("Personality");
		specialTrait = (SpecialTrait)(int)GetMeta("SpecialTrait");
		budget = (float)GetMeta("Budget");

		GD.Print(GetMeta("Name") + "'s willingness is to buy " + Inventory.itemsList[0].GetMeta("Name") + ": " + CalculatePurchaseWillingess(Inventory.itemsList[0], 0));
	}

	public override void _Process(double delta) {
	}

	public float CalculatePurchaseWillingess(Item item, float mod) {

		float willingness = 0.0f;

		if((int)GetMeta("Class") == (int)item.GetMeta("ItemType")) {
			willingness += 0.3f;
			GD.Print("class/item match (+0.3)");
		}


		if((int)item.GetMeta("ItemType") == (int)ItemType.Potion) {
			willingness += 0.15f;
		}

		if((int)item.GetMeta("ItemType") == (int)ItemType.Potion) {
			willingness += 0.15f;
		}


		if((int)GetMeta("Personality") == (int)Personality.Jolly) {
			willingness += 0.1f;
			GD.Print("Jolly (+0.1)");
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.3f;

			willingness += (float) (-(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/(int)item.GetMeta("Rarity")+1.0f)/1.5f;
		}
		if((int)GetMeta("Personality") == (int)Personality.Cheapskate) {
			willingness -= 0.2f;
			GD.Print("Cheapskate (-0.3)");			
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.3f;

			willingness += (float) -(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/(5*(int)item.GetMeta("Rarity"))+0.2f;
		}
		if((int)GetMeta("Personality") == (int)Personality.Foolhardy) {
			willingness += 0.1f;
			GD.Print("Foolhardy (+0.1)");

			willingness += (float) (Math.Pow(Math.E, (double)(int)(item.GetMeta("Rarity")))-1.5f)/2;
		}
		if((int)GetMeta("Personality") == (int)Personality.Cowardly) {
			if((int)item.GetMeta("isCursed") != 0) willingness -= 0.5f;

			willingness += (float) -(Math.Log10((double)(int)(item.GetMeta("Rarity"))))/5*(int)item.GetMeta("Rarity")+0.2f;
		}

		if((int)GetMeta("SpecialTrait") == (int)item.GetMeta("ItemTrait")) {
			willingness += 0.5f;
			GD.Print("Special trait match (+0.5)");
		}

		if((int)item.GetMeta("ItemTrait") == (int)ItemTrait.Broken) {
			willingness -= (int)item.GetMeta("Rarity") == (int)Rarity.Legendary ? 0.1f : 0.3f;
		}

		return willingness + mod;
	}

	public bool modifyQuitPercentage(float willingness) {
		Random random = new Random();
		float invWillingness = (0.55f - willingness)/1.6f;
		quitPercentage += invWillingness;
		double percantageRoll = random.NextDouble();
		if(percantageRoll < invWillingness) {
			return true;
		}
		return false;
	}
	
	public (bool, String) buy(float willingness, float price) {
		Random random = new Random();
		double buyPercentage = random.NextDouble();
		if(buyPercentage > (1.0-willingness) && price <= budget) {
			return (true, "This seems perfect, I will take it off your hands\n\n");
		} else if(buyPercentage < willingness && price > budget) {
			return (false, "I would buy if it fell within my price range");
		} else {
			return (false, "I don't see that I need it specifically");
		}
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

