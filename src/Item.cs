using Godot;
using System;

public partial class Item : Control {
	ItemType type;
	Rarity rarity;
	float cost;
	String name;
	Texture2D image;
	TextureRect itemImage;
	Label itemNameLabel;
	Label Cost;
	Label Type;
	Label Rarity;
	VBoxContainer Vbox;
	public override void _Ready() {
		Vbox = GetNode<VBoxContainer>("VBoxContainer");
		Size = Vbox.Size;
		CustomMinimumSize = Vbox.Size;
		itemImage = GetNode<TextureRect>("VBoxContainer/TextureRect");
		itemNameLabel = GetNode<Label>("VBoxContainer/Label");
		Cost = GetNode<Label>("VBoxContainer/Cost");
		Type = GetNode<Label>("VBoxContainer/HBoxContainer/Type");
		Rarity = GetNode<Label>("VBoxContainer/HBoxContainer/Rarity");

		type = (ItemType)(int)GetMeta("ItemType");
		Type.Text = type.ToString();
		rarity = (Rarity)(int)GetMeta("Rarity");
		Rarity.Text = rarity.ToString();
		cost = (float)GetMeta("Cost");
		Cost.Text = "Cost: $" + cost.ToString();

		name = (String)GetMeta("Name");
		itemNameLabel.Text = name;
		image = (Texture2D)GetMeta("Image");
		itemImage.Texture = image;
	}
	 
	public void setSize() {
		Vbox = GetNode<VBoxContainer>("VBoxContainer");
		Size = Vbox.Size;
		CustomMinimumSize = Vbox.Size;
	}

	public override void _Process(double delta) {
	}
}

//Enums For Items
public enum ItemType {
	Weapon = 1, Potion = 2, Armour = 3,
}

enum Rarity {
	Common = 1, Uncommon = 2, Rare = 3, Legendary = 4,
}