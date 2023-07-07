using Godot;
using System;

public partial class UISwitcher : Control {
    Button inventoryButton;
    Inventory inventory;
    Control customerView;
    bool isInventoryOpen = false;
    public override void _Ready() {
        inventory = GetNode<Inventory>("../Inventory");
        inventory.Hide();
        customerView = GetNode<Control>("../CustomerView");
        inventoryButton = GetNode<Button>("HBoxContainer/InventoryButton");
        inventoryButton.Pressed += () => {
            isInventoryOpen = !isInventoryOpen;
            if(isInventoryOpen) {
                inventory.Show();
            } else {
                inventory.Hide();
            }
        };
        base._Ready();
    }
}
