using Godot;
using System;

public partial class Player : Control {
	Texture2D portraitImage;
	String name;
	Personality personality;
	PlayerClass playerClass;
	Label classLabel;
	Label nameLabel;
	TextureRect portraitRect;
	public override void _Ready() {
		portraitImage = (Texture2D)GetMeta("Portrait");
		name = (String)GetMeta("Name");
		personality = (Personality)(int)GetMeta("Personality");
		playerClass = (PlayerClass)(int)GetMeta("Class");
	}

	public override void _Process(double delta) {
	}
}

enum Personality {
	Jolly = 1, Cheapskate = 2,
}

enum PlayerClass {
	Archer = 1, Melee = 2, Mage = 3,
}