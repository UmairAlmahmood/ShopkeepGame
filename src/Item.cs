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
	MarginContainer marginContainer;
	public NinePatchRect border;
	bool isMouseInside = false;
	public bool isPressable = true;
	public override void _Ready() {
		marginContainer = GetNode<MarginContainer>("MarginContainer");
		Size = marginContainer.Size;
		CustomMinimumSize = marginContainer.Size;
		itemImage = GetNode<TextureRect>("MarginContainer/VBoxContainer/TextureRect");
		itemNameLabel = GetNode<Label>("MarginContainer/VBoxContainer/Label");
		Cost = GetNode<Label>("MarginContainer/VBoxContainer/Cost");
		Type = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/Type");
		Rarity = GetNode<Label>("MarginContainer/VBoxContainer/HBoxContainer/Rarity");
		border = GetNode<NinePatchRect>("Border");
		border.Hide();
		border.Size = Size;

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
		
		MouseEntered += () => {
			if(isPressable) {
                border.Show();
                isMouseInside = true;
			}
		};
		MouseExited += () => {
			border.Hide();
			isMouseInside = false;
		};
	}
	 
	public void setSize() {
		marginContainer = GetNode<MarginContainer>("MarginContainer");
		Size = marginContainer.Size;
		CustomMinimumSize = marginContainer.Size;
	}

	public override void _Process(double delta) {
		if(Input.IsActionJustReleased("MouseClicked") && isMouseInside && isPressable) {
			EmitSignal(SignalName.ClickedOnItem, this);
		}
	}

	[Signal]
	public delegate void ClickedOnItemEventHandler(Item item);
}

//Enums For Items
public enum ItemType {
	Weapon = 1, Potion = 2, Armour = 3,
}

enum Rarity {
	Common = 1, Uncommon = 2, Rare = 3, Legendary = 4,
}