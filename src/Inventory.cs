using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control {
	public static List<Item> itemsList = new List<Item>();
    [Export]
    int numItems = 5;
    PackedScene itemScene;
    Texture2D itemTexture;
    GridContainer inventoryMenu;
    Control itemPlace;
    bool isSellingItem = false;
    public override void _Ready() {
        itemPlace = GetNode<Control>("../ItemPlace");
		Random randomNumGen = new Random();
        itemScene = ResourceLoader.Load<PackedScene>("res://scenes/Item.tscn");
        inventoryMenu = GetNode<GridContainer>("MarginContainer/GridContainer");
        for(int i = 0; i<numItems; i++) {

            Item item = itemScene.Instantiate<Item>();
            ItemType itemType = (ItemType)(int)randomNumGen.Next(1, 6);
            item.SetMeta("ItemType", (int)itemType);


            itemTexture = itemType switch {
                ItemType.Sword => ResourceLoader.Load<Texture2D>("res://assets/ItemImages/basic-sword-texture.png"),
                ItemType.Bow => ResourceLoader.Load<Texture2D>("res://assets/ItemImages/basic-bow-texture.png"),
                ItemType.Potion => ResourceLoader.Load<Texture2D>("res://assets/ItemImages/basic-potion-texture-1.png"),
                _ => ResourceLoader.Load<Texture2D>("res://assets/ItemImages/basic-sword-texture.png"),
            };

            int rarity = randomNumGen.Next(1, 21);
            if(rarity <= 10) rarity = 1;
            else if(rarity <= 15) rarity = 2;
            else if(rarity <= 18) rarity = 3;
            else  rarity = 4;
            

            item.SetMeta("Rarity", rarity);
            int cursed = (randomNumGen.NextDouble() < 0.9 ? (int)Cursed.None : randomNumGen.Next(1, 5));
            int trait = rarity>1 ? (randomNumGen.NextDouble() < 0.5 ? (int)ItemTrait.None : randomNumGen.Next(1, 6)) : (int)ItemTrait.None;

            int baseLinePrice = itemType switch {
                ItemType.Sword => 50,
                ItemType.Bow => 50,
                ItemType.Staff => 60,
                ItemType.Potion => 10,
                ItemType.Armour => 75,
                _ => 50,
            };
            item.SetMeta("Cost", (float)Math.Round(((randomNumGen.NextDouble()*10 + baseLinePrice)*Math.Pow(rarity, 3)), 2));
            item.SetMeta("Image", itemTexture);
            item.SetMeta("isCursed", cursed);

            item.SetMeta("Name", itemNameGen.GenerateItemName((int)itemType, rarity, cursed, trait));

            itemsList.Add(item);
            item.setSize();
            inventoryMenu.AddChild(item);
            item.ClickedOnItem += ItemClickedOn;
        }
        base._Ready();
    }
    
    void ItemClickedOn(Item item) {
        itemsList.Remove(item);
        inventoryMenu.RemoveChild(item);
        item.isPressable = false;
        item.Position = Vector2.Zero;
        item.border.Hide();
        itemPlace.AddChild(item);
    }

}
