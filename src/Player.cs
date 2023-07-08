using Godot;
using System;

public partial class Player : Control {
	Texture2D portraitImage;
	public String name;
	public Personality personality;
	public PlayerClass playerClass;
	public Label classLabel;
	Label nameLabel;
	TextureRect portraitRect;
	public override void _Ready() {
		classLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Class");
		nameLabel = GetNode<Label>("VBoxContainer/HBoxContainer/Name");
		portraitRect = GetNode<TextureRect>("VBoxContainer/Portrait");
		

		portraitImage = (Texture2D)GetMeta("Portrait");
		portraitRect.Texture = portraitImage;
		name = (String)GetMeta("Name");
		nameLabel.Text = name;
		playerClass = (PlayerClass)(int)GetMeta("Class");
		classLabel.Text = playerClass.ToString();

		personality = (Personality)(int)GetMeta("Personality");
	}

	public override void _Process(double delta) {
	}
}

public enum Personality {
	Jolly = 1, Cheapskate = 2, Foolhardy = 3, Cowardly = 4,
}

public enum PlayerClass {
	Archer = 1, Warrior = 2, Mage = 3,
}

public enum SpecialTrait {
	VsUndead = 1, VsGoblins = 2, Dungeoneer = 3, Diver = 4, VsDragon = 5, CurseHunter = 6,
}