using Godot;
using System;

public partial class Item : Node2D {
	ItemType type;
	Rarity rarity;
	float cost;
	String name;
	Texture2D image;
	public override void _Ready() {
		type = (ItemType)(int)GetMeta("ItemType");
		rarity = (Rarity)(int)GetMeta("Rarity");
		cost = (float)GetMeta("Cost");
		name = (String)GetMeta("Name");
		image = (Texture2D)GetMeta("Image");
	}

	public override void _Process(double delta) {
	}
}

//Enums For Items
enum ItemType {
	Weapon = 1, Potion = 2,
}

enum Rarity {
	Common = 1, Uncommon = 2, Rare = 3, Legendary = 4,
}