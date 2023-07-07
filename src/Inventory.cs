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
    public override void _Ready() {
		Random randomNumGen = new Random();
        itemScene = ResourceLoader.Load<PackedScene>("res://scenes/Item.tscn");
        itemTexture = ResourceLoader.Load<Texture2D>("res://assets/ItemImages/GenericJellyItem.png");
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
            item.SetMeta("Cost", (float)Math.Round(((randomNumGen.NextDouble() + baseLinePrice)*Math.Pow(rarity, 3)), 2));
            item.SetMeta("Image", itemTexture);
            itemsList.Add(item);
            item.setSize();
            inventoryMenu.AddChild(item);
        }
        base._Ready();
    }
}
