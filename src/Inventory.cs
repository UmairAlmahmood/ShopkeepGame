using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Control {
	List<Item> itemsList = new List<Item>();
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
        itemTexture = ResourceLoader.Load<Texture2D>("res://assets/ItemImages/basic-sword-texture.png");
        inventoryMenu = GetNode<GridContainer>("MarginContainer/GridContainer");
        for(int i = 0; i<numItems; i++) {
            Item item = itemScene.Instantiate<Item>();
            item.SetMeta("Name", "Hello How Are You");
            ItemType itemType = (ItemType)(int)randomNumGen.Next(1, 4);
            item.SetMeta("ItemType", (int)itemType);
            int rarity = randomNumGen.Next(1, 5);
            item.SetMeta("Rarity", rarity);
            
            int baseLinePrice = itemType switch {
                ItemType.Weapon => 50,
                ItemType.Potion => 10,
                ItemType.Armour => 75,
                _ => 50,
            };
            item.SetMeta("Cost", (float)Math.Round(((randomNumGen.NextDouble()*10 + baseLinePrice)*Math.Pow(rarity, 3)), 2));
            item.SetMeta("Image", itemTexture);
            item.SetMeta("isCursed", (randomNumGen.NextDouble() < 0.9 ? (int)Cursed.None : randomNumGen.Next(1, 5)));
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
