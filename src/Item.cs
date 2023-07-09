using Godot;
using System;
using System.Text.RegularExpressions;

public partial class Item : Control {
	ItemType type;
	Rarity rarity;
	public float cost;
	String name;
	Texture2D image;
	TextureRect itemImage;
	Label itemNameLabel;
	LineEdit Cost;
	Label Type;
	Label Rarity;
	Label Cursed;
	MarginContainer marginContainer;
	public NinePatchRect border;
	public bool isMouseInside = false;
	public Cursed cursed = (Cursed)0;
	public bool isPressable = true;
	ShaderMaterial defaultMat;
	ShaderMaterial legendary;
	String oldText = "";
	public override void _Ready() {
		defaultMat = ResourceLoader.Load<ShaderMaterial>("res://assets/shaders/Default.tres");
		legendary = ResourceLoader.Load<ShaderMaterial>("res://assets/shaders/Legendary.material");
		marginContainer = GetNode<MarginContainer>("MarginContainer");
		Size = marginContainer.Size;
		CustomMinimumSize = marginContainer.Size;
		itemImage = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");
		itemNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		Cost = GetNode<LineEdit>("MarginContainer/VBoxContainer/Cost");
		Cost.TextChanged += costChanged;
		Type = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/Type");
		Rarity = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/Rarity");
		Cursed = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/Is Cursed");
		border = GetNode<NinePatchRect>("Border");
		border.Hide();
		border.Size = Size;

		type = (ItemType)(int)GetMeta("ItemType");
		Type.Text = type.ToString();
		rarity = (Rarity)(int)GetMeta("Rarity");
		Rarity.Text = rarity.ToString();
		cost = (float)GetMeta("Cost");
		Cost.PlaceholderText = "Estimated Cost: $" + cost.ToString();
		cursed = (Cursed)(int)GetMeta("isCursed");
		Cursed.Text = (int)cursed != 0 ? "Cursed" : "";

		name = (String)GetMeta("Name");
		itemNameLabel.Text = name;
		image = (Texture2D)GetMeta("Image");
		itemImage.Texture = image;
		
		if(rarity == global::Rarity.Legendary) {
			itemImage.Material = legendary;
		} else {
			itemImage.Material = defaultMat;
		}
		
		MouseEntered += MouseEnteredHandler;
		MouseExited += mouseExitedHandler;
	}

    private void costChanged(string newText) {
		if(Regex.IsMatch(newText, "[^0-9.]")) {
			Cost.Text = oldText;
		} else {
			oldText = newText;
			float newCost = (float)Double.Parse(newText);
			cost = newCost;
		}
    }

    void mouseExitedHandler() {
        border.Hide();
        isMouseInside = false;

    }
	
	void MouseEnteredHandler() {
		if(isPressable) {
			border.Show();
			isMouseInside = true;
		} else {
			isMouseInside = false;
		}
	}
	 
	public void setSize() {
		marginContainer = GetNode<MarginContainer>("MarginContainer");
		Size = marginContainer.Size;
		CustomMinimumSize = marginContainer.Size;
	}

	public override void _Process(double delta) {
		if(Input.IsActionJustReleased("MouseClicked") && isMouseInside && isPressable) {
			GD.Print("AAAAAAAAAAAHHHHHHHHHHHHHHhh", this.name);
			EmitSignal(SignalName.ClickedOnItem, this);
		}
	}

	[Signal]
	public delegate void ClickedOnItemEventHandler(Item item);
}

//Enums For Items
public enum ItemType {
	Sword = 1, Bow = 2, Staff = 3, Potion = 4, Armour = 5,
}

enum Rarity {
	Common = 1, Uncommon = 2, Rare = 3, Legendary = 4,
}

public enum Cursed {
	None = 0, HemorrhageMoney = 1, PoisonIvy = 2, Tipsy = 3, DoubleToil = 4, DoubleTrouble = 5,
}

public enum ItemTrait {
	VsUndead = 1, VsGoblins = 2, Dungeoneer = 3, Diver = 4, VsDragon = 5, Broken = 6, None = 7,
}