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

            item.SetMeta("Name", GenerateItemName((int)itemType, rarity, cursed, trait));

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

        

    private static String GenerateItemName(int ItemType, int Rarity, int Cursed, int ItemTrait) {

        String[] SwordSegment = {"Blade", "Sword", "Nail", "Tooth"};
        String[] BowSegment = {"Bow", "Longbow", "Shortbow"};
        String[] StaffSegment = {"Staff", "Sceptre", "Rod"};
        String[] ArmourSegment = {"Mail", "Armour", "Plate"};
        String[] PotionSegment = {"Potion", "Brew", "Elixir"};

        String[] VsUndeadPrefix = {"Blessed", "Silvered",};
        String[] VsUndeadSuffix = {"of Silver", "of Zombie Slaying"};
        String[] VsUndeadLegendarySwords = {"Grave Warning", "Holy Avenger"};
        String[] VsUndeadLegendaryBows = {"Mortal Reminder"};
        String[] VsUndeadLegendaryStaffs = {"Bone's Bane"};
        String[] VsUndeadLegendaryArmour = {"Grave Guardian's Plate"};
        String[] VsUndeadLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};


        String[] VsGoblinsPrefix = {"Goblin Slayer's"};
        String[] VsGoblinsSuffix = {"of Goblin Slaying"};
        String[] VsGoblinsLegendarySwords = {"Gobsmacker 3000", "Orcrist", "Glamdring"};
        String[] VsGoblinsLegendaryBows = {"Gobsniper"};
        String[] VsGoblinsLegendaryStaffs = {"Nilbog's Staff"};
        String[] VsGoblinsLegendaryArmour = {"Armour of the Tall"};
        String[] VsGoblinsLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};

        String[] DiverPrefix = {"Marine", "Tidal"};
        String[] DiverSuffix = {"of the Sea", "of the Tide"};
        String[] DiverLegendarySwords = {"Shark-Tooth"};
        String[] DiverLegendaryBows = {"Sea-King's Stormbow"};
        String[] DiverLegendaryStaffs = {"Tsunami"};
        String[] DiverLegendaryArmour = {"Shark-Bait's Plate"};
        String[] DiverLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};


        String[] VsDragonPrefix = {"Dragon-Scale", "Fireproof"};
        String[] VsDragonSuffix = {"of Wyrm-Slaying", "of the Dragon-Slayer"};
        String[] VsDragonLegendarySwords = {"Gurthang"};
        String[] VsDragonLegendaryBows = {"Bard's Bow"};
        String[] VsDragonLegendaryStaffs = {"Wyrm's Bane"};
        String[] VsDragonLegendaryArmour = {"Lava-Wader's Mail"};
        String[] VsDragonLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};


        String[] BrokenPrefix = {"Defective", "Ancient", "Incomplete", "Coupon for a"};
        String[] BrokenSuffix = {"Voucher", "of the Dragon-Slayer"};
        String[] BrokenLegendarySwords = {"Hero's Sword"};
        String[] BrokenLegendaryBows = {"Odysseus' Unstrung Bow"};
        String[] BrokenLegendaryStaffs = {"Shattered Sceptre of the Fallen Mage-King"};
        String[] BrokenLegendaryArmour = {"Rusted Armour of the Hero"};
        String[] BrokenLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};


        String[] DungeoneerPrefix = {"Adventurer's", "Shining", "Spelunker's", "Cave-In Proof"};
        String[] DungeoneerSuffix = {"of the Spelunker", "of the Dungeoneer", "of Darkvision"};
        String[] DungeoneerLegendarySwords = {"Master Dungeoneer's Sword"};
        String[] DungeoneerLegendaryBows = {"Master Dungeoneer's Bow"};
        String[] DungeoneerLegendaryStaffs = {"Master Dungeoneer's Staff"};
        String[] DungeoneerLegendaryArmour = {"Master Dungeoneer's Armour"};
        String[] DungeoneerLegendaryPotions = {"Potion that Makes You Very Good at Killing Goblins"};

        String[] LegendarySwords = {"Excalibur", "Sting"};
        String[] LegendaryBows = {"Fail-Not"};
        String[] LegendaryStaffs = {"Grey Wizard's Staff"};
        String[] LegendaryArmour = {"Mithril Chain-Mail"};
        String[] LegendaryPotions = {"Ambrosia", "Panacea"};

        String[] UncommonPrefix = {"Trusty", "Stalwart", "Journeyman's"};
        String[] UncommonSuffix = {"of the Adventurer"};

        String[] CommonPrefix = {"Ordinary", "Legendary", "Squire's", "Steel", "Handy"};
        String[] CommonSuffix = {"of the Amateur Adventurer", ""};





        Random randint = new Random();

        String itemName = "";

        if(Rarity == 4) {
            if(ItemTrait == 1) {
                itemName = ItemType switch {
                    1 => VsUndeadLegendarySwords[randint.Next(0, VsUndeadLegendarySwords.Length)],
                    2 => VsUndeadLegendaryBows[randint.Next(0, VsUndeadLegendaryBows.Length)],
                    3 => VsUndeadLegendaryStaffs[randint.Next(0, VsUndeadLegendaryStaffs.Length)],
                    4 => VsUndeadLegendaryPotions[randint.Next(0, VsUndeadLegendaryPotions.Length)],
                    5 => VsUndeadLegendaryArmour[randint.Next(0, VsUndeadLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 2) {
                itemName = ItemType switch {
                    1 => VsGoblinsLegendarySwords[randint.Next(0, VsGoblinsLegendarySwords.Length)],
                    2 => VsGoblinsLegendaryBows[randint.Next(0, VsGoblinsLegendaryBows.Length)],
                    3 => VsGoblinsLegendaryStaffs[randint.Next(0, VsGoblinsLegendaryStaffs.Length)],
                    4 => VsGoblinsLegendaryPotions[randint.Next(0, VsGoblinsLegendaryPotions.Length)],
                    5 => VsGoblinsLegendaryArmour[randint.Next(0, VsGoblinsLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 3) {
                itemName = ItemType switch {
                    1 => DungeoneerLegendarySwords[randint.Next(0, DungeoneerLegendarySwords.Length)],
                    2 => DungeoneerLegendaryBows[randint.Next(0, DungeoneerLegendaryBows.Length)],
                    3 => DungeoneerLegendaryStaffs[randint.Next(0, DungeoneerLegendaryStaffs.Length)],
                    4 => DungeoneerLegendaryPotions[randint.Next(0, DungeoneerLegendaryPotions.Length)],
                    5 => DungeoneerLegendaryArmour[randint.Next(0, DungeoneerLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 4) {
                itemName = ItemType switch {
                    1 => DiverLegendarySwords[randint.Next(0, DiverLegendarySwords.Length)],
                    2 => DiverLegendaryBows[randint.Next(0, DiverLegendaryBows.Length)],
                    3 => DiverLegendaryStaffs[randint.Next(0, DiverLegendaryStaffs.Length)],
                    4 => DiverLegendaryPotions[randint.Next(0, DiverLegendaryPotions.Length)],
                    5 => DiverLegendaryArmour[randint.Next(0, DiverLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 5) {
                itemName = ItemType switch {
                    1 => VsDragonLegendarySwords[randint.Next(0, VsDragonLegendarySwords.Length)],
                    2 => VsDragonLegendaryBows[randint.Next(0, VsDragonLegendaryBows.Length)],
                    3 => VsDragonLegendaryStaffs[randint.Next(0, VsDragonLegendaryStaffs.Length)],
                    4 => VsDragonLegendaryPotions[randint.Next(0, VsDragonLegendaryPotions.Length)],
                    5 => VsDragonLegendaryArmour[randint.Next(0, VsDragonLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 6) {
                itemName = ItemType switch {
                    1 => BrokenLegendarySwords[randint.Next(0, BrokenLegendarySwords.Length)],
                    2 => BrokenLegendaryBows[randint.Next(0, BrokenLegendaryBows.Length)],
                    3 => BrokenLegendaryStaffs[randint.Next(0, BrokenLegendaryStaffs.Length)],
                    4 => BrokenLegendaryPotions[randint.Next(0, BrokenLegendaryPotions.Length)],
                    5 => BrokenLegendaryArmour[randint.Next(0, BrokenLegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            if(ItemTrait == 7) {
                itemName = ItemType switch {
                    1 => LegendarySwords[randint.Next(0, LegendarySwords.Length)],
                    2 => LegendaryBows[randint.Next(0, LegendaryBows.Length)],
                    3 => LegendaryStaffs[randint.Next(0, LegendaryStaffs.Length)],
                    4 => LegendaryPotions[randint.Next(0, LegendaryPotions.Length)],
                    5 => LegendaryArmour[randint.Next(0, LegendaryArmour.Length)],
                    _ => "Warning: Glorbus",
                };
            }
            return itemName;
        };




        bool isPrefixed = randint.Next(0, 2) == 1;

        if(isPrefixed) {
            itemName += ItemTrait switch {
                1 => VsUndeadPrefix[randint.Next(0, VsUndeadPrefix.Length)],
                2 => VsGoblinsPrefix[randint.Next(0, VsGoblinsPrefix.Length)],
                3 => DungeoneerPrefix[randint.Next(0, DungeoneerPrefix.Length)],
                4 => DiverPrefix[randint.Next(0, DiverPrefix.Length)],
                5 => VsDragonPrefix[randint.Next(0, VsDragonPrefix.Length)],
                6 => BrokenPrefix[randint.Next(0, BrokenPrefix.Length)],
                7 => Rarity == 1 ? CommonPrefix[randint.Next(0, CommonPrefix.Length)] : UncommonPrefix[randint.Next(0, UncommonPrefix.Length)],
                _ => "Warning: Glorbus",
            } + " ";

            
        }

        itemName += ItemType switch {
            1 => SwordSegment[randint.Next(0, SwordSegment.Length)],
            2 => BowSegment[randint.Next(0, BowSegment.Length)],
            3 => StaffSegment[randint.Next(0, StaffSegment.Length)],
            4 => PotionSegment[randint.Next(0, PotionSegment.Length)],
            5 => ArmourSegment[randint.Next(0, ArmourSegment.Length)],
            _ => "Warning: Glorbus",
        };

        if(!isPrefixed) {
            itemName += " " + ItemTrait switch {
                1 => VsUndeadSuffix[randint.Next(0, VsUndeadSuffix.Length)],
                2 => VsGoblinsSuffix[randint.Next(0, VsGoblinsSuffix.Length)],
                3 => DungeoneerSuffix[randint.Next(0, DungeoneerSuffix.Length)],
                4 => DiverSuffix[randint.Next(0, DiverSuffix.Length)],
                5 => VsDragonSuffix[randint.Next(0, VsDragonSuffix.Length)],
                6 => BrokenSuffix[randint.Next(0, BrokenSuffix.Length)],
                7 => Rarity == 1 ? CommonSuffix[randint.Next(0, CommonSuffix.Length)] : UncommonSuffix[randint.Next(0, UncommonSuffix.Length)],
                _ => "Warning: Glorbus",
            };
        }


        itemName = " " + Cursed switch {
            0 => itemName,
            1 => "Greedy " + itemName,
            2 => "Poisonous " + itemName,
            3 => "Tipsy " + itemName,
            4 => "Lazy " + itemName,
            5 => "Troublesome " + itemName,
            _ => "Warning: Glorbus",
        };


        return itemName;
    }

}
